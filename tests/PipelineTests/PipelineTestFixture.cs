using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PipelineTests.Helpers;
using Schuster;

namespace PipelineTests
{
	[TestFixture]
	public class PipelineTestFixture
	{
		[SetUp]
		public void Setup()
		{
			_mockCalls = new Mock<IMockCalls>();
			LuaTestExtension = new LuaTestExtension(_mockCalls.Object);
			ExtensionCollection = new ExtensionCollection();
			ExtensionCollection.AddLuaApi(LuaTestExtension);
			Pipeline = new Pipeline(ExtensionCollection);

			_statusUpdates = new List<PipelineStatus>();

			_mockCalls.Setup(m => m.Call(It.IsAny<PipelineStatus>()))
				.Callback<PipelineStatus>(update => { _statusUpdates.Add(update); });

			Pipeline.StatusUpdate += status => { _mockCalls.Object.Call(status); };
		}

		protected Mock<IMockCalls> _mockCalls;
		protected LuaTestExtension LuaTestExtension;
		protected ExtensionCollection ExtensionCollection;
		protected Pipeline Pipeline;

		protected List<PipelineStatus> _statusUpdates;
	}
}