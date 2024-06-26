﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SarahSamehEcommerceWebApi.Models;

namespace SarahSamehEcommerceWebApi.Features.Account;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;

    public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
    {
        this._userManager = userManager;
        this._config = config;
    }

    [HttpPost("register")] //api/account/register
    public async Task<IActionResult> Register(RegisterUserDto userDto)
    {
        if (ModelState.IsValid)
        {

            ApplicationUser appuser = new ApplicationUser()
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = userDto.Password
            };
            //create account in db
            IdentityResult result = await _userManager.CreateAsync(appuser, userDto.Password);
            if (result.Succeeded)
            {
                return Ok("Account Created");
            }
            return BadRequest(result.Errors);

        }
        return BadRequest(ModelState);
    }

    [HttpPost("login")] //api/account/login
    public async Task<IActionResult> Login(LoginUserDto userDto)
    {
        if (ModelState.IsValid)
        {
            //return null not Found or AppUser if Found 
            ApplicationUser? userFromDb = await _userManager.FindByNameAsync(userDto.UserName);

            if (userFromDb != null)
            {
                //found in db
                //check pass

                bool found = await _userManager.CheckPasswordAsync(userFromDb, userDto.Password);

                if (found)
                {
                    //create token

                    List<Claim> myclaim = new List<Claim>();
                    myclaim.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));
                    myclaim.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id));
                    myclaim.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); //jti ==> Token id


                    var roles = await _userManager.GetRolesAsync(userFromDb);
                    foreach (var role in roles)
                    {
                        myclaim.Add(new Claim(ClaimTypes.Role, role));
                    }
                    var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecritKey"])); //"ssssssssssssssssssssssssssssssssssssssssssssssssssssss"

                    SigningCredentials signingCredentials =
                        new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256);


                    JwtSecurityToken myToken = new JwtSecurityToken(
                        issuer: _config["JWT:ValidIss"],
                        audience: _config["JWT:ValidAud"],
                        expires: DateTime.Now.AddHours(1),
                        claims: myclaim,
                        signingCredentials: signingCredentials

                    );
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(myToken),
                        expired = myToken.ValidTo
                    });
                }
            }
            //null no user in db 
            return Unauthorized("Invalid Account");
        }
        return BadRequest(ModelState);
    }
}