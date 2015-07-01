/*
 * Copyright (C) 2015 Colin Mackie.
 * This software is distributed under the terms of the GNU General Public License.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#include "ykcore_lcl.h"
#include "ykcore_backend.h"
#include "yktsd.h"

/* To get modhex and crc16 */
#include "yubikey.h"

#include "ykdef.h"

#include <stdio.h>
#ifndef _WIN32
#include <Windows.h>
#include <tchar.h>
#include <unistd.h>
#define Sleep(x) usleep((x)*1000)
#endif

#ifdef YK_DEBUG
static void _yk_hexdump(void *, int);
#endif

#define ERROR_OK 0
#define ERROR_CANNOT_OPEN_KEY 1
#define ERROR_INVALID_SLOT 2
#define ERROR_CHALLENGE_TOO_BIG 3
#define ERROR_IN_CHALLENGE_RESPONSE 4
#define ERROR_INVALID_KEYSIZE 5
#define ERROR_WRITING_SLOT 6

/// Info structure for GetInfo
struct info_st {
	YK_STATUS status;
	uint32_t serial;
	uint32_t pid;
};
typedef struct info_st YK_INFO;

typedef struct config_st YK_CONFIG;

int _lastError = 0;

///
/// Initialize the underlying library
///
__declspec(dllexport) int YkInit()
{
	return yk_init();
}
///
/// Cleanup the underlying library
///
__declspec(dllexport) void YkRelease()
{
	yk_release();
}

///
/// Get the Info about the first YubiKey
///
__declspec(dllexport) int GetInfo(YK_INFO *info)
{
	int pid;
	YK_KEY* key = yk_open_first_key_with_pid(&pid);
	if (key == NULL)
	{
		_lastError = yk_errno;
		return ERROR_CANNOT_OPEN_KEY;
	}

	yk_get_status(key, &info->status);

	yk_get_serial(key, 0, 0, &info->serial);

	info->pid = pid;

	yk_close_key(key);

	return 0;
}

///
/// Check if the specified slot has been configured
///
__declspec(dllexport) int IsSlotConfigured(int slot, bool* result)
{
	YK_INFO info;
	YK_KEY* key;

	key = yk_open_first_key();
	if (key == NULL)
	{
		_lastError = yk_errno;
		return ERROR_CANNOT_OPEN_KEY;
	}

	yk_get_status(key, &info.status);

	if (slot == 1 && (info.status.touchLevel & CONFIG1_VALID) != 0)
	{
		*result = true;
	}
	else if (slot == 2 && (info.status.touchLevel & CONFIG2_VALID) != 0)
	{
		*result = true;
	}
	else
	{
		*result = false;
	}

	yk_close_key(key);

	return 0;
}

///
/// Perform a ChallengeResponse operation on specified slot
///
__declspec(dllexport) int ChallengeResponse(uint32_t slot, BOOL may_block,
		const unsigned char *challenge, uint32_t challenge_len, 
		unsigned char *response, uint32_t response_len)
{
	uint8_t command;
	YK_KEY* key;
	int result;

	if (slot == 1)
	{
		command = SLOT_CHAL_HMAC1;
	}
	else if (slot == 2)
	{
		command = SLOT_CHAL_HMAC2;
	}
	else
	{
		// invalid slot
		return ERROR_INVALID_SLOT;
	}

	if (challenge_len > 64)
	{
		// cannot be bigger than 64 bytes
		return ERROR_CHALLENGE_TOO_BIG;
	}

	key = yk_open_first_key();
	if (key == NULL)
	{
		return ERROR_CANNOT_OPEN_KEY;
	}

	result = 0;
	if (!yk_challenge_response(key, command, may_block, challenge_len, challenge, response_len, response))
	{
		_lastError = yk_errno;
		result = ERROR_IN_CHALLENGE_RESPONSE;
	}

	yk_close_key(key);

	return result;
}

///
/// Set the ChallengeResponse key on specified slot
///
__declspec(dllexport) int SetChallengeResponse(uint32_t slot, const unsigned char* secret, uint32_t keysize, BOOL userpress, unsigned char* access_code)
{
	YK_CONFIG config;
	uint8_t command;
	YK_KEY* key;
	int result = 0;

	memset(&config, 0, sizeof(config));

	if (slot == 1)
	{
		command = SLOT_CONFIG;
	}
	else if (slot == 2)
	{
		command = SLOT_CONFIG2;
	}
	else
	{
		// invalid slot
		return ERROR_INVALID_SLOT;
	}

	if (keysize == KEY_SIZE_OATH)
	{
		// put first 16 in key and last 4 in uid
		memcpy(config.key, secret, KEY_SIZE);
		memcpy(config.uid, secret + KEY_SIZE, keysize - KEY_SIZE);

		config.tktFlags |= TKTFLAG_CHAL_RESP;
		config.cfgFlags |= CFGFLAG_CHAL_HMAC;
		config.cfgFlags |= CFGFLAG_HMAC_LT64;
	}
	else if (keysize == KEY_SIZE)
	{
		memcpy(config.key, secret, KEY_SIZE);

		config.tktFlags |= TKTFLAG_CHAL_RESP;
		config.cfgFlags |= CFGFLAG_CHAL_YUBICO;
	}
	else
	{
		// invalid key size
		return ERROR_INVALID_KEYSIZE;
	}

	if (userpress)
	{
		config.cfgFlags |= CFGFLAG_CHAL_BTN_TRIG;
	}

	key = yk_open_first_key();
	if (key == NULL)
	{
		// cannot open
		_lastError = yk_errno;
		return ERROR_CANNOT_OPEN_KEY;
	}

	if (!yk_write_command(key, &config, command, access_code ? access_code : NULL))
	{
		result = ERROR_WRITING_SLOT;
		_lastError = yk_errno;
	}
	yk_close_key(key);

	return result;
}

///
/// Get the last error from the YubiKey internals
///
__declspec(dllexport) int LastError()
{
	return _lastError;
}
