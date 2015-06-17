/* ykparse.c --- Example command line interface for authentication token.
 *
 * Written by Simon Josefsson <simon@josefsson.org>.
 * Copyright (c) 2006-2012 Yubico AB
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 *    * Redistributions of source code must retain the above copyright
 *      notice, this list of conditions and the following disclaimer.
 *
 *    * Redistributions in binary form must reproduce the above
 *      copyright notice, this list of conditions and the following
 *      disclaimer in the documentation and/or other materials provided
 *      with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */

#include "yubikey.h"

#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>

int
main (int argc, char *argv[])
{
  uint8_t buf[128];
  uint8_t key[YUBIKEY_KEY_SIZE];
  char *aeskey, *token;
  yubikey_token_st tok;

  /* Parse command line parameters. */
  if (argc < 2)
    {
      printf ("Usage: %s <aeskey> <token>\n", argv[0]);
      printf (" AESKEY:\tHex encoded AES-key.\n");
      printf (" TOKEN:\t\tModHex encoded token.\n");
      return EXIT_FAILURE;
    }

  aeskey = argv[1];
  token = argv[2];

  if (strlen (aeskey) != 32)
    {
      printf ("error: Hex encoded AES-key must be 32 characters.\n");
      return EXIT_FAILURE;
    }

  if (strlen (token) > 32)
    {
      printf ("warning: overlong token, ignoring prefix: %.*s\n",
	      (int) strlen (token) - 32, token);
      token = token + (strlen (token) - 32);
    }

  if (strlen (token) != 32)
    {
      printf ("error: ModHex encoded token must be 32 characters.\n");
      return EXIT_FAILURE;
    }

  /* Debug. */
  printf ("Input:\n");
  printf ("  token: %s\n", token);

  yubikey_modhex_decode ((char *) key, token, YUBIKEY_KEY_SIZE);

  {
    size_t i;
    printf ("          ");
    for (i = 0; i < YUBIKEY_KEY_SIZE; i++)
      printf ("%02x ", key[i] & 0xFF);
    printf ("\n");
  }

  printf ("  aeskey: %s\n", aeskey);

  yubikey_hex_decode ((char *) key, aeskey, YUBIKEY_KEY_SIZE);

  {
    size_t i;
    printf ("          ");
    for (i = 0; i < YUBIKEY_KEY_SIZE; i++)
      printf ("%02x ", key[i] & 0xFF);
    printf ("\n");
  }

  /* Pack up dynamic password, decrypt it and verify checksum */
  yubikey_parse ((uint8_t *) token, key, &tok);

  printf ("Output:\n");
  {
    size_t i;
    printf ("          ");
    for (i = 0; i < YUBIKEY_BLOCK_SIZE; i++)
      printf ("%02x ", ((uint8_t *) & tok)[i] & 0xFF);
    printf ("\n");
  }

  printf ("\nStruct:\n");
  /* Debug */
  {
    size_t i;
    printf ("  uid: ");
    for (i = 0; i < YUBIKEY_UID_SIZE; i++)
      printf ("%02x ", tok.uid[i] & 0xFF);
    printf ("\n");
  }
  printf ("  counter: %d (0x%04x)\n", tok.ctr, tok.ctr);
  printf ("  timestamp (low): %d (0x%04x)\n", tok.tstpl, tok.tstpl);
  printf ("  timestamp (high): %d (0x%02x)\n", tok.tstph, tok.tstph);
  printf ("  session use: %d (0x%02x)\n", tok.use, tok.use);
  printf ("  random: %d (0x%02x)\n", tok.rnd, tok.rnd);
  printf ("  crc: %d (0x%04x)\n", tok.crc, tok.crc);

  printf ("\nDerived:\n");
  printf ("  cleaned counter: %d (0x%04x)\n",
	  yubikey_counter (tok.ctr), yubikey_counter (tok.ctr));
  yubikey_modhex_encode ((char *) buf, (char *) tok.uid, YUBIKEY_UID_SIZE);
  printf ("  modhex uid: %s\n", buf);
  printf ("  triggered by caps lock: %s\n",
	  yubikey_capslock (tok.ctr) ? "yes" : "no");
  printf ("  crc: %04X\n", yubikey_crc16 ((void *) &tok, YUBIKEY_KEY_SIZE));

  printf ("  crc check: ");
  if (yubikey_crc_ok_p ((uint8_t *) & tok))
    {
      printf ("ok\n");
      return EXIT_SUCCESS;
    }

  printf ("fail\n");
  return EXIT_FAILURE;
}
