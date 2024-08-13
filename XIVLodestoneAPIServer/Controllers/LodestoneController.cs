using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
namespace XIVLodestoneAPIServer.Controllers;


[Route("[controller]")]
public class LodestoneController : ControllerBase
{
    private readonly ILogger<PlayerSearchQuery> _logger;
    
    public LodestoneController(ILogger<PlayerSearchQuery> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public string Get()
    {
        return "Hello World!";
    }
    
    [HttpGet("CharacterId")]
    public async Task<IActionResult> GetCharacterId(PlayerSearchQuery query)
    {
        if (query.FirstName == null || query.LastName == null)
        {
            return BadRequest("Please provide a first and last name.");
        }
        else
        {
            var webContent = GetWebContent(query);
            var node = webContent.DocumentNode.SelectSingleNode("//a[@class='entry__link']");
            var characterURL = node.GetAttributeValue("href", "");

            if (string.IsNullOrEmpty(characterURL))
            {
                return NotFound("Character not found.");
            }
            
            var id = characterURL.Split(new[] { "/lodestone/character/", "/" }, StringSplitOptions.RemoveEmptyEntries)[0];
            return Ok(id);
        }
    }
    
    [HttpGet("FreeCompany")]
    public async Task<IActionResult> GetFreeCompany(PlayerSearchQuery query)
    {
        if (query.FirstName == null || query.LastName == null)
        {
            return BadRequest("Please provide a first and last name.");
        }
        else
        {
            var webContent = GetWebContent(query);
            var node = webContent.DocumentNode.SelectSingleNode("//a[@class='entry__freecompany__link']/span");
            
            if (node == null)
            {
                return NotFound("Free Company not found.");
            }
            else
            {
                var freeCompanyName = node.InnerText;
                return Ok(freeCompanyName);

            }
        }
    }
    
    private HtmlDocument GetWebContent(PlayerSearchQuery query)
    {
        // Example Query In Browser
        // https://na.finalfantasyxiv.com/lodestone/character/?q=Art+Bayard&worldname=Carbuncle&classjob=&race_tribe=&blog_lang=ja&blog_lang=en&blog_lang=de&blog_lang=fr&order=
        var baseURL = "https://na.finalfantasyxiv.com/lodestone/character/?q=";
        var endQueryURL = "&worldname=Carbuncle&classjob=&race_tribe=&blog_lang=ja&blog_lang=en&blog_lang=de&blog_lang=fr&order=";
        var nameParameter = "";
    
        if (query.FirstName == null || query.LastName == null)
        {
            _logger.LogError("Missing FirstName or LastName in GetWebContent");
            return null;
        }
        else
        {
            nameParameter = query.FirstName + "+" + query.LastName;
            var fullURL = baseURL + nameParameter + endQueryURL;
            var web = new HtmlWeb();
            var webContent = web.Load(fullURL);
            return webContent;
        }
    }
    
    [HttpGet("Ping")]
    public string AdditionalEndpoint()
    {
        return "Pong!";
    }

}