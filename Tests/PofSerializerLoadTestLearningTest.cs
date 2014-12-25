using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Dargon.Hydar.PortableObjects;
using Dargon.PortableObjects;
using ItzWarty;
using NMockito;
using Xunit;

namespace Dargon.Hydar {
   public class PofSerializerLoadTestLearningTest : NMockitoInstance {
      private readonly IPofSerializer serializer;

      public PofSerializerLoadTestLearningTest() {
         var pofContext = new TestPofContext();
         serializer = new PofSerializer(pofContext);
      }

      [Fact]
      public void Run() {
         Console.WriteLine("Generating Test Data.");
         var testData = Util.Generate(100000, GenerateAccountEntry);

         Console.WriteLine("Running Serializer Load Test.");
         var testDataBox = new ArrayBox<AccountEntry>(testData);

         for (var i = 0; i < 3; i++) {
            Stopwatch stopwatch = new Stopwatch().With(x => x.Start());
            using (var ms = new MemoryStream(testData.Length * 128)) {
               using (var writer = new BinaryWriter(ms, Encoding.UTF8, true)) {
                  serializer.Serialize(writer, testDataBox);
               }
               Console.WriteLine("Write completed at {0} ms.", stopwatch.ElapsedMilliseconds);
               ms.Position = 0;
               using (var reader = new BinaryReader(ms, Encoding.UTF8, true)) {
                  serializer.Deserialize<ArrayBox<AccountEntry>>(reader);
               }
               Console.WriteLine("Read completed at {0} ms.", stopwatch.ElapsedMilliseconds);
            }

            // this load-test is nonfatal as it is intended to let us understand libdpo's performance.
            // also, the spec only requires that deserializing 100000 elements takes less than 1
            // second, so the fact that we can serialize and deserialize under that is great.
            if (stopwatch.ElapsedMilliseconds <= 1000) {
               Console.WriteLine("Load Test Passed!");
            } else {
               Console.WriteLine("Load Test Failed!");
            }
         }
      }

      private static AccountEntry GenerateAccountEntry(int i) {
         return new AccountEntry(Guid.NewGuid(), "Account " + i, "Test" + i + "@example.com", Guid.NewGuid());
      }
   }
}
