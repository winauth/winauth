/* ykhex.c --- Implementation of hex encoding/decoding
 *
 * Written by Simon Josefsson <simon@josefsson.org>.
 * Copyright (c) 2006-2014 Yubico AB
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

static const char hex_trans[] = "0123456789abcdef";
static const char modhex_trans[] = YUBIKEY_MODHEX_MAP;

static void
_yubikey_encode (char *dst, const char *src, size_t srcSize,
		 const char *trans)
{
  while (srcSize--)
    {
      *dst++ = trans[(*src >> 4) & 0xf];
      *dst++ = trans[*src++ & 0xf];
    }

  *dst = '\0';
}

static void
_yubikey_decode (char *dst, const char *src, size_t dstSize,
		 const char *trans)
{
  char b;
  int flag = 0;
  const char *p1;

  if (strlen (src) % 2 == 1)
    {
      flag = !flag;
    }

  for (; *src && dstSize > 0; src++)
    {
      if ((p1 = strchr (trans, *src)) == NULL)
	b = 0;
      else
	b = (char) (p1 - trans);

      if ((flag = !flag))
	*dst = b;
      else
	{
	  *dst = (*dst << 4) | b;
	  dst++;
	  dstSize--;
	}
    }
  while (dstSize--)
    *dst++ = 0;
}

static int
_yubikey_p (const char *str, const char *trans)
{
  for (; *str; str++)
    if (strchr (trans, *str) == NULL)
      return 0;

  return 1;
}

void
yubikey_hex_encode (char *dst, const char *src, size_t srcSize)
{
  _yubikey_encode (dst, src, srcSize, hex_trans);
}

void
yubikey_hex_decode (char *dst, const char *src, size_t dstSize)
{
  _yubikey_decode (dst, src, dstSize, hex_trans);
}

int
yubikey_hex_p (const char *str)
{
  return _yubikey_p (str, hex_trans);
}


void
yubikey_modhex_encode (char *dst, const char *src, size_t srcSize)
{
  _yubikey_encode (dst, src, srcSize, modhex_trans);
}

void
yubikey_modhex_decode (char *dst, const char *src, size_t dstSize)
{
  _yubikey_decode (dst, src, dstSize, modhex_trans);
}

int
yubikey_modhex_p (const char *str)
{
  return _yubikey_p (str, modhex_trans);
}
