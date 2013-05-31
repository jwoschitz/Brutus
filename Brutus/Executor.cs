/*
 * Created by SharpDevelop.
 * User: janosch.woschitz@gmail.com
 */
using System;
using System.Linq;

namespace Brutus
{
	public class Executor
	{   
		private char[] keyspace;
		
		private int indexOfLastCharInKeyspace;
		
		private char[] lastComputedKey;
		
  		public Executor(char[] keyspace)
		{
			if (keyspace.Length == 0) {
				throw new ArgumentException("Keyspace needs to contain any characters.");
			}
			this.keyspace = keyspace;
			// minor performance improvement
			this.indexOfLastCharInKeyspace = keyspace.Length - 1;
		}
		
		public void Reset() {
			lastComputedKey = null;
		}
		
		public char[] ComputeNextKey() {
			if (lastComputedKey == null) {
				lastComputedKey = new char[] { keyspace[0] };
				return lastComputedKey;
			}
			
			int index = lastComputedKey.Length - 1;
			int keyspaceIndex = this.getKeyspaceIndexFor(lastComputedKey[index]);
			
			while (keyspaceIndex == indexOfLastCharInKeyspace) {
				lastComputedKey[index] = keyspace[0];
				if (index == 0) {
					// preserve the old length as new index
					index = lastComputedKey.Length;
					Array.Resize<char>(ref lastComputedKey, lastComputedKey.Length + 1); 
					lastComputedKey[index] = keyspace[0];
					return lastComputedKey;
				} else {
					index--;
					keyspaceIndex = this.getKeyspaceIndexFor(lastComputedKey[index]);
				}
			}
			
			   lastComputedKey[index] = keyspace[++keyspaceIndex];
			   return lastComputedKey;
		}
		
		public char[] getKeyspace() {
			return keyspace;
		}
		
		private int getKeyspaceIndexFor(char value) {
			return Array.IndexOf(keyspace, value);
		}
	}
}
