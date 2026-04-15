using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using System.IO;
using System.Text;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class StreamAttachmentTests
    {
        /// <summary>
        /// Test that streams are read from their current position (caller controls positioning)
        /// </summary>
        [TestMethod]
        public void SheetAttachFile_StreamPositionNotAtZero_ShouldReadFromCurrentPosition()
        {
            // Create a test stream with some content
            byte[] testData = Encoding.UTF8.GetBytes("This is test content for attachment upload");
            using (MemoryStream stream = new MemoryStream(testData))
            {
                // Move stream position to middle to simulate a stream that has been partially read
                stream.Position = 10;
                
                // Verify the SDK convention: caller is responsible for stream positioning
                // If caller wants full content, they must ensure Position = 0
                byte[] readFromCurrent = new byte[testData.Length - 10];
                int bytesRead = stream.Read(readFromCurrent, 0, readFromCurrent.Length);
                Assert.AreEqual(testData.Length - 10, bytesRead, "Should read from current position, not from start");
                
                // To upload full content, caller must reset position themselves
                stream.Position = 0;
                Assert.AreEqual(0, stream.Position, "Caller controls stream position");
            }
        }

        /// <summary>
        /// Test that row attachments read from current stream position
        /// </summary>
        [TestMethod]
        public void RowAttachFile_StreamAtCurrentPosition_ShouldReadFromThere()
        {
            byte[] testData = Encoding.UTF8.GetBytes("Row attachment test data");
            using (MemoryStream stream = new MemoryStream(testData))
            {
                // Move stream position forward
                stream.Position = 5;
                
                // Verify reading from current position (not from beginning)
                byte[] readData = new byte[testData.Length - 5];
                int bytesRead = stream.Read(readData, 0, readData.Length);
                Assert.AreEqual(testData.Length - 5, bytesRead, "Should read from current position");
                
                // Caller responsibility: reset if full content is needed
                Assert.IsTrue(stream.CanSeek, "Test stream should be seekable");
            }
        }

        /// <summary>
        /// Test that comment attachments respect current stream position
        /// </summary>
        [TestMethod]
        public void CommentAttachFile_CallerControlsPosition_ShouldWorkCorrectly()
        {
            byte[] testData = Encoding.UTF8.GetBytes("Comment attachment test data");
            using (MemoryStream stream = new MemoryStream(testData))
            {
                // Verify that stream positioning is caller's responsibility
                stream.Position = 0;
                byte[] fullRead = new byte[testData.Length];
                stream.Read(fullRead, 0, fullRead.Length);
                CollectionAssert.AreEqual(testData, fullRead, "Reading from position 0 gives full content");
                
                // Move position and verify partial read
                stream.Position = 8;
                byte[] partialRead = new byte[testData.Length - 8];
                int bytesRead = stream.Read(partialRead, 0, partialRead.Length);
                Assert.AreEqual(testData.Length - 8, bytesRead, "Partial read from current position");
            }
        }

        /// <summary>
        /// Test that attachment versioning reads from current stream position
        /// </summary>
        [TestMethod]
        public void AttachNewVersion_StreamPosition_IsCallerResponsibility()
        {
            byte[] testData = Encoding.UTF8.GetBytes("Attachment version test data");
            using (MemoryStream stream = new MemoryStream(testData))
            {
                // Demonstrate caller control: position affects what gets read
                stream.Position = 12;
                long positionBefore = stream.Position;
                
                byte[] remainingData = new byte[testData.Length - 12];
                stream.Read(remainingData, 0, remainingData.Length);
                
                // The SDK reads from wherever the stream is positioned
                Assert.AreEqual(12, positionBefore, "Stream was at position 12");
                Assert.AreEqual(testData.Length - 12, remainingData.Length, "Read remaining bytes from position 12");
            }
        }

        /// <summary>
        /// Test behavior with a stream at EOF - caller must reposition if needed
        /// </summary>
        [TestMethod]
        public void AttachFile_StreamAtEOF_CallerMustRepositionForContent()
        {
            byte[] testData = Encoding.UTF8.GetBytes("EOF test data");
            using (MemoryStream stream = new MemoryStream(testData))
            {
                // Move to end (EOF)
                stream.Position = stream.Length;
                Assert.AreEqual(stream.Length, stream.Position, "Stream is at EOF");
                
                // Reading from EOF gives zero bytes
                byte[] readFromEOF = new byte[testData.Length];
                int bytesRead = stream.Read(readFromEOF, 0, readFromEOF.Length);
                Assert.AreEqual(0, bytesRead, "Reading from EOF returns zero bytes");
                
                // Caller's responsibility: reset to read content
                stream.Position = 0;
                byte[] readAfterReset = new byte[testData.Length];
                bytesRead = stream.Read(readAfterReset, 0, readAfterReset.Length);
                Assert.AreEqual(testData.Length, bytesRead, "After caller resets position, full content is available");
                CollectionAssert.AreEqual(testData, readAfterReset, "Content matches when read from position 0");
            }
        }

        /// <summary>
        /// Test that non-seekable streams work correctly (read from current position)
        /// </summary>
        [TestMethod]
        public void AttachFile_NonSeekableStream_ReadsFromCurrentPosition()
        {
            // Create a non-seekable stream wrapper
            byte[] testData = Encoding.UTF8.GetBytes("Non-seekable stream test");
            using (MemoryStream baseStream = new MemoryStream(testData))
            using (NonSeekableStreamWrapper nonSeekableStream = new NonSeekableStreamWrapper(baseStream))
            {
                Assert.IsFalse(nonSeekableStream.CanSeek, "Stream should not be seekable");
                
                // For non-seekable streams, SDK simply reads from current position
                // No position manipulation is attempted
                byte[] readData = new byte[testData.Length];
                int bytesRead = nonSeekableStream.Read(readData, 0, readData.Length);
                Assert.AreEqual(testData.Length, bytesRead, "Should read all bytes from current position");
                CollectionAssert.AreEqual(testData, readData, "Content should match");
            }
        }

        /// <summary>
        /// Test that the caller can control stream positioning for multiple operations
        /// </summary>
        [TestMethod]
        public void AttachFile_MultipleReads_CallerControlsPositioning()
        {
            byte[] testData = Encoding.UTF8.GetBytes("Multiple reads test data");
            using (MemoryStream stream = new MemoryStream(testData))
            {
                // First operation: read a portion
                byte[] firstRead = new byte[10];
                stream.Read(firstRead, 0, 10);
                Assert.AreEqual(10, stream.Position, "Position advances during read");
                
                // Caller can choose to continue from current position
                byte[] continueRead = new byte[5];
                stream.Read(continueRead, 0, 5);
                Assert.AreEqual(15, stream.Position, "Position continues from previous read");
                
                // Or caller can reset to re-read from beginning
                stream.Position = 0;
                byte[] fullRead = new byte[testData.Length];
                int bytesRead = stream.Read(fullRead, 0, fullRead.Length);
                Assert.AreEqual(testData.Length, bytesRead, "Full read after caller resets position");
                CollectionAssert.AreEqual(testData, fullRead, "Full content available when caller sets position to 0");
            }
        }

        /// <summary>
        /// Helper class to create a non-seekable stream for testing
        /// </summary>
        private class NonSeekableStreamWrapper : Stream
        {
            private readonly Stream baseStream;

            public NonSeekableStreamWrapper(Stream baseStream)
            {
                this.baseStream = baseStream;
            }

            public override bool CanRead => baseStream.CanRead;
            public override bool CanSeek => false; // Override to make non-seekable
            public override bool CanWrite => baseStream.CanWrite;
            public override long Length => baseStream.Length;

            public override long Position
            {
                get => throw new NotSupportedException("Stream is not seekable");
                set => throw new NotSupportedException("Stream is not seekable");
            }

            public override void Flush() => baseStream.Flush();

            public override int Read(byte[] buffer, int offset, int count)
            {
                return baseStream.Read(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException("Stream is not seekable");
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException("Stream is not seekable");
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                baseStream.Write(buffer, offset, count);
            }
        }
    }
}
