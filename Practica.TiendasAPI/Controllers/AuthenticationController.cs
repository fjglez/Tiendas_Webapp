using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Practica.TiendasAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Practica.TiendasAPI.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;


        public AuthenticationController(SignInManager<IdentityUser> signInManager, IMapper mapper,
                        UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenAsync(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                          new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                          new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                          _config["Tokens:Issuer"],
                          _config["Tokens:Audience"],
                          claims,
                          expires: DateTime.UtcNow.AddMinutes(30),
                          signingCredentials: creds);

                        var res = new
                        {
                            username = model.Username,
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created("", res);
                    }
                }
                return Unauthorized();
            }

            return BadRequest();

        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<IdentityUser>(model);
                var res = await _userManager.CreateAsync(user, model.Password);

                if (res.Succeeded)
                {
                    return NoContent();
                }
                // Si el usuario ya existe devuelve una respuesta 409 Conflict
                else if (res.Errors.Any(x => x.Code == "DuplicateUserName"))
                {
                    return Conflict();
                }
                // Devuelve los errores del registro en una respuesta 400 Bad Request
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return BadRequest();
        }
    }
}
