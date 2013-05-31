/*
 * Created by SharpDevelop.
 * User: janosch.woschitz@gmail.com
 */
using System;
using System.Linq;
using Brutus;
using NUnit.Framework;

namespace Brutus.Test
{
	[TestFixture]
	public class ExecutorTest
	{		
		[Test]
		public void TestDoNotInitializeIfKeyspaceIsEmpty()
		{
			Assert.Throws<ArgumentException>(delegate { new Executor(new char[] {}); });
		}
		
		[Test]
		public void TestComputeNextKey() {
			var executor = new Executor(new char[] {'a', 'b', 'c'});
			Assert.AreEqual(new char[] {'a'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'b'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'c'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'a', 'a'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'a', 'b'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'a', 'c'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'b', 'a'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'b', 'b'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'b', 'c'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'c', 'a'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'c', 'b'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'c', 'c'}, executor.ComputeNextKey());
			Assert.AreEqual(new char[] {'a', 'a', 'a'}, executor.ComputeNextKey());
			for(var i = 0; i < 350; i++) {
				executor.ComputeNextKey();
			}
			Assert.AreEqual(new char[] {'a', 'a', 'a', 'a', 'a', 'a'}, executor.ComputeNextKey());
		}
		
		[Test]
		public void TestComputeNextKeyDeterminism() {
			var keyspace = new char[] {'a', 'b', 'c'};
			var executor = new Executor(keyspace);
			
			for(var keyLength = 1; keyLength < 10; keyLength++) {
				var numberOfPossibleKeys = (int) Math.Pow(keyspace.Length, keyLength);
				char[] computedKey = new char[0];
				for(var i = 0; i < numberOfPossibleKeys; i++) {
					// compute all keys for the current key length
					computedKey = executor.ComputeNextKey();
				}
				// the last computed key for this key length should contain only the last character of the keyspace
				var lastComputedKeyShouldBe = createFilledArray<char>('c', keyLength);
				Assert.AreEqual(lastComputedKeyShouldBe, computedKey);
			}
		}
		
		private T[] createFilledArray<T>(T value, int length) {
			return Enumerable.Range(0, length).Select(c => value).ToArray();
		}
	}
}
