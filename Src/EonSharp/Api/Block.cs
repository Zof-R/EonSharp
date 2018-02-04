using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	public class Block
	{
		public BigInteger CumulativeDifficulty { get; set; }
		public string GenerationSignature { get; set; }
		public string Generator { get; set; }
		public int Height { get; set; }
		public string Id { get; set; }
		public string Prev { get; set; }
		public string Signature { get; set; }
		public string Snapshot { get; set; }
		public int Timestamp { get; set; }
		public int Version { get; set; }



		//		{{
		//  "cumulativeDifficulty": "20351076777",
		//  "generationSignature": "edbfadad6b05d03f129c4ef3491137e68b6a11061c606e23becef61f30e0c82c54097113b4f4f7d91684472ce07bcabf1dc71a0692054d786dc10515de7b030e",
		//  "generator": "EON-D3GHJ-3YKU2-5XKTK",
		//  "height": 43281,
		//  "id": "EON-B-NBDR6-7S5NT-VJ6NS",
		//  "prev": "EON-B-26DR6-7ND7F-7H7ZZ",
		//  "signature": "400cb921bac51810b3c8e92fdb72dcf5b3d7f96a087b6b20b567441012ba9127a3e4caa38105964ea8495db6235ed5572acb599951f88cdd076160ebc3875100",
		//  "snapshot": "e8183869b4675d06dcdcb8a5e045d51f2e00116b00467420c988e031df0b7c43a5aee3f55b047bd0a5242dcc683de82df5d7f0521f29efc22699ca42bb869f5c",
		//  "timestamp": 1514908980,
		//  "version": 2
		//}}

		//PreparedStatement saveStatement = db.prepareStatement(
		//			"INSERT OR REPLACE INTO \"block\" (\"id\", \"version\", \"timestamp\", \"previousBlock\", \"generator\", \"generationSignature\", \"blockSignature\", \"height\", \"nextBlock\", \"cumulativeDifficulty\", \"snapshot\")\n VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);");
		//	synchronized(saveStatement)
		//{

		//	saveStatement.setLong(1, block.getID());
		//	saveStatement.setInt(2, block.getVersion());
		//	saveStatement.setInt(3, block.getTimestamp());
		//	saveStatement.setLong(4, block.getPreviousBlock());
		//	saveStatement.setLong(5, block.getSenderID());
		//	saveStatement.setString(6, Format.convert(block.getGenerationSignature()));
		//	saveStatement.setString(7, Format.convert(block.getSignature()));
		//	saveStatement.setInt(8, block.getHeight());
		//	saveStatement.setLong(9, 0L);
		//	saveStatement.setString(10, block.getCumulativeDifficulty().toString());
		//	saveStatement.setString(11, Format.convert(block.getSnapshot()));
		//	saveStatement.executeUpdate();

		//}

	}
}
