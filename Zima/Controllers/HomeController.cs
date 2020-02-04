using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Zima.Data;
using Zima.Models;

namespace Zima.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly string _trustedKey;
        private readonly string _path;
        private readonly string _site;
        private readonly PacketRepository _packetRepository;
        public HomeController(IConfiguration configuration, PacketRepository packetRepository)
        {
            _trustedKey = configuration["Application:KeyAuthorization:TrustedKey"];
            _path = configuration["Application:KeyAuthorization:Path"];
            _site = configuration["Application:KeyAuthorization:KeyValidationSite"];
            _packetRepository = packetRepository;
        }

        [HttpGet("fetch/{name}")]
        public ActionResult Fetch(string name)
        {
            var package = _packetRepository.FindLatest(name);
            if (package == null)
            {
                return NotFound();
            }
            else
            {
                return PhysicalFile(package.Locate(), "application/x-zip-compressed");
            }
        }

        [HttpGet("fetch/{name}/{version}")]
        public ActionResult FetchByVersion(string name, string version)
        {
            var package = _packetRepository.Find(name, version);
            if (package == null)
            {
                return NotFound();
            }
            else
            {
                return PhysicalFile(package.Locate(), "application/x-zip-compressed");
            }
        }

        [HttpGet("inspect/{name}")]
        public ActionResult<Package> Inspect(string name)
        {
            var res = _packetRepository.FindLatest(name);
            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("inspect/{name}/{version}")]
        public ActionResult<Package> InspectByVersion(string name, string version)
        {
            var res = _packetRepository.Find(name, version);
            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("list")]
        public IEnumerable<Package> List()
        {
            return _packetRepository.All();
        }

        [HttpGet("list/{name}")]
        public IEnumerable<Package> ListByName([FromRoute]string name)
        {
            return _packetRepository.List(name);
        }

        [HttpPost("publish")]
        [RequestSizeLimit(64 * 1024 * 1024)]
        public async Task<ActionResult<Package>> PublishAsync([FromQuery]string key)
        {
            string op;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    string res = wc.DownloadString(string.Format(_site, key));
                    if (string.IsNullOrEmpty(_path))
                    {
                        op = res;
                    }
                    else
                    {
                        JObject obj = JsonConvert.DeserializeObject<JObject>(res);
                        op = obj[_path].ToObject<string>();
                    }
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await Request.Body.CopyToAsync(stream);
                        ZipArchive zip = new ZipArchive(stream);
                        StreamReader reader = new StreamReader(zip.GetEntry("package.json").Open());
                        Package model = JsonConvert.DeserializeObject<Package>(reader.ReadToEnd());
                        model.UploadDate = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
                        if (_packetRepository.Add(model, op))
                        {
                            using (FileStream fs = new FileStream(model.Locate(), FileMode.Create, FileAccess.Write))
                            {
                                stream.Position = 0;
                                await stream.CopyToAsync(fs);
                            }
                            return Ok(model);
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
                catch (WebException)
                {
                    return Unauthorized();
                }
                catch (JsonException)
                {
                    return StatusCode(500);
                }
            }

        }

        [HttpGet("validate")]
        public ActionResult ValidateKey(string key)
        {
            return _trustedKey == key ? Ok(key) : (ActionResult)NotFound(key);
        }
    }
}