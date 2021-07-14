using Microsoft.AspNetCore.Mvc;
using Api_project.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController:Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApplicationContext _applicationContext;

        public AuthenticationController(ApplicationContext context,UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration =     configuration;
            _applicationContext = context;
        }
        // [HttpPost]
        // [Route("post")]
        // public async Task<ActionResult> PostRegister([FromBody]Register register)
        // {

        //     if (register == null)
        //     {
        //         return NotFound("Employee data is not supplied");
        //     }
        //     //System.Console.WriteLine($"{register.FirstName}");
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }
        //     await _applicationContext.register.AddAsync(register);
        //     await _applicationContext.SaveChangesAsync();
        //     return Ok(register);
        // }

        [HttpPost]
        public async Task<object> Register([FromBody] Register model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email, 
                Email = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _applicationContext.register.AddAsync(model);
             await _applicationContext.SaveChangesAsync();
                
                await _signInManager.SignInAsync(user, false);
                return await GenerateJwtToken(model.Email, user);
            }
            
            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [HttpGet]
        [Route("get")]
        public ActionResult<IEnumerable<Department>> GetDept()
        {
            return _applicationContext.department.ToList();
        }

        [HttpGet]
        [Route("getDes")]
        public ActionResult<IEnumerable<Designation>> GetDesignation()
        {
            return _applicationContext.designation.ToList();
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public ActionResult<Designation> GetById(int id)
        {
            if (id <= 0)
            {
                return NotFound("Employee id must be higher than zero");
            }
            Designation designation = _applicationContext.designation.FirstOrDefault(s => s.DesignationId == id);
            if (designation == null)
            {
                return NotFound("Employee not found");
            }
            return Ok(designation);
        }

        [HttpGet]
        [Route("getbydeptid/{id}")]
        public ActionResult<Designation> GetByDeptId(int id)
        {
            if (id <= 0)
            {
                return NotFound("department id must be higher than zero");
            }
            
            List<Designation> designations=_applicationContext.designation.Where(x=>x.DepartmentId==id).ToList<Designation>();

            if (designations == null)
            {
                return NotFound("department not found");
            }
            return Ok(designations);
        }

        [HttpPost]
        [Route("login")]
        public async Task<object> Login([FromBody] Login model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            
            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return await GenerateJwtToken(model.Email, appUser);
            }
            
            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [Authorize]
        [HttpGet]
        public async Task<object> Protected()
        {
            return "Protected area";
        }
        
        private async Task<object> GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JWT:JwtIssuer"],
                _configuration["JWT:JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}