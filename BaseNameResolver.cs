﻿using System;
using System.Diagnostics.Contracts;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace UnrealPlugin
{
	internal abstract class BaseNameResolver : INameResolver
	{
		protected readonly RemoteProcess process;
		protected readonly BaseConfig config;

		protected BaseNameResolver(RemoteProcess process, BaseConfig config)
		{
			Contract.Requires(process != null);
			Contract.Requires(config != null);

			this.process = process;
			this.config = config;
		}

		public string ReadNameOfObject(IntPtr address)
		{
			var nameIndex = ReadNameIndexFromObject(address);
			if (nameIndex < 1)
			{
				return null;
			}

			var nameEntryPtr = ReadNameEntryPtr(nameIndex);
			if (!nameEntryPtr.MayBeValid())
			{
				return null;
			}

			return ReadNameFromNameEntry(nameEntryPtr, nameIndex);
		}

		protected virtual int ReadNameIndexFromObject(IntPtr address)
		{
			return process.ReadRemoteInt32(address + config.UObjectNameOffset);
		}

		protected abstract IntPtr ReadNameEntryPtr(int index);

		protected abstract string ReadNameFromNameEntry(IntPtr nameEntryPtr, int index);
	}
}