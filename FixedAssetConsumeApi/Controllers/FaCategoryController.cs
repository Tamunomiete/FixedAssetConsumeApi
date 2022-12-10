using FixedAssetConsumeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;

namespace FixedAssetConsumeApi.Controllers
{
	public class FaCategoryController : Controller
	{
		private readonly ILogger<FaCategoryController> _logger;
		string baseurl = "https://localhost:44308/";
		public FaCategoryController(ILogger<FaCategoryController> logger)
		{
			_logger = logger;
		}
		public async Task<IActionResult> Index()
		{
			// calling the web api and populating data in view using datable
			List<TblFaCategoryEntity> listofTblFacategory = new List<TblFaCategoryEntity>();
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseurl);   // passing in the base url here
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // setting the header
				HttpResponseMessage getData = await client.GetAsync("/api/tblFaCategory/GetTblFacategory");

				if (getData.IsSuccessStatusCode)  // if we get staus code 200 sucessful code then 
				{
					string result = getData.Content.ReadAsStringAsync().Result;  // read as string to fetch us the json object
					listofTblFacategory = JsonConvert.DeserializeObject<List<TblFaCategoryEntity>>(result); // pass in data table here and esult
				}
				else
				{
					Console.WriteLine("Error in calling web api");
				}
			
			}
			return View(listofTblFacategory);
		}
		public async Task<ActionResult<string>> AddFacategory(TblFaCategoryEntity tblFacategory)
		{
			TblFaCategoryEntity obj = new TblFaCategoryEntity()
			{
				Id = tblFacategory.Id,
				CatCode = tblFacategory.CatCode,
				CatDesc = tblFacategory.CatDesc,
				Status = tblFacategory.Status,
				UserId = tblFacategory.UserId,
				AuthId = tblFacategory.AuthId

			};
			if (tblFacategory.CatCode != null)
			{
				using (var client = new HttpClient())
				{
					obj.CatCode = tblFacategory.CatCode;
					client.BaseAddress = new Uri(baseurl + "/api/tblFaCategory/AddTblFacategory");   // passing in the base url here
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // setting the header
					HttpResponseMessage getData = await client.PostAsJsonAsync("/api/tblFaCategory/AddTblFacategory", obj);// 

					if (getData.IsSuccessStatusCode)  // if we get staus code 200 sucessful code then 
					{
						return RedirectToAction("Index", "FaCategory");  // read as string to fetch us the json object

					}
					else
					{
						Console.WriteLine("Error in calling web api");
					}
					ViewData.Model = tblFacategory;
				}
			}

			return View();

		}
		public async Task<IActionResult> EditFacategory(int id)
		{
			TblFaCategoryEntity tblFacategory = new TblFaCategoryEntity();
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.BaseAddress = new Uri(baseurl);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));


				var responseTask = await client.GetAsync("/api/tblFaCategory/GetTblFacategoryById"+id);
				if (responseTask.IsSuccessStatusCode)
				{
					var item = responseTask.Content.ReadAsStringAsync().Result;
					tblFacategory = JsonConvert.DeserializeObject<TblFaCategoryEntity>(item);
				}

				//create  a post method to update the data

			}
			return View(tblFacategory);
		}

		[HttpPost]
		public async Task<IActionResult> Update(TblFaClassEntity tblFaclass)
		{
			string CustomMessage;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.BaseAddress = new Uri(baseurl);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));
				var responseMessage = await client.PutAsJsonAsync("/api/tblFaCategory/UpdateTblFacategory" + tblFaclass.Id, tblFaclass);
				if (responseMessage.IsSuccessStatusCode)
				{
					var item = responseMessage.Content.ReadAsStringAsync().Result;
					CustomMessage = JsonConvert.DeserializeObject<string>(item);

				}


			}
			return View("Index");
		}


	}
}