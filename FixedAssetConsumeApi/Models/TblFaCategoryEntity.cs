
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace FixedAssetConsumeApi.Models
{
	public class TblFaCategoryEntity
	{

		public int Id { get; set; }
		public string CatCode { get; set; }
		public string CatDesc { get; set; }

		public int? Status { get; set; }
		public string UserId { get; set; }
		public string AuthId { get; set; }
	}
}
