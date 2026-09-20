using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegRoma.Data;
using RegRoma.DTOs;
using RegRoma.Models;
using RegRoma.Services;

namespace RegRoma.Controllers;

[ApiController]
[Route("api.[controller]")]
public class AuthController : ControllerBase
{
  private readonly AppDbContext _db;
  private readonly PasswordServices _ps;
  private readonly JwtServices _jwts;

  public AuthController(AppDbContext db, PasswordServices ps, JwtServices jwts)
  {
    _db = db;
    _ps = ps;
    _jwts = jwts;
  }

  //reg
  [HttpPost("register")]
  [AllowAnonymous]
  public async Task<IActionResult> Register(RegReq req)
  {
    var exists = await _db.Users.AnyAsync(user => user.Email == req.Email);

    if (exists)
    {
      return BadRequest("пользователь с такой почтой уже существует");
    }

    var user = new User
    {
      Email = req.Email,
      PasswordHash = _ps.PasswordHash(req.Password)
    };
    _db.Add(user);
    await _db.SaveChangesAsync();

    return Ok("пользователь создан");
  }
  //login
  public async Task<ActionResult<AuthReq>> Login(LoginRequest request)
  {
    var user = await _db.Users.FirstOrDefaultAsync(user => user.Email == request.Email);

    if (user == null)
    {
      return Unauthorized("неверная почта или пароль");
    }

    var valid = _ps.VerifyPassword(request.Password, user.PasswordHash);

    if (!valid)
    {
      return Unauthorized("неверная почта или пароль");
    }

    var token = _jwts.CreateToken(user);

    return Ok(new AuthReq
    {
      Token = token,
      Email = user.Email
    });
  } 
}
