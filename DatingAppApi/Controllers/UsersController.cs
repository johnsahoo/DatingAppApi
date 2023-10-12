using AutoMapper;
using DatingAppApi.Data;
using DatingAppApi.DTOs;
using DatingAppApi.Entities;
using DatingAppApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.Controllers
{
    
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return  Ok(users);
        }

        
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return  await _userRepository.GetMemberAsync(username);
        }

        //private readonly DataContext _context;

        //public UsersController(DataContext context)
        //{
        //    _context = context;
        //}

        //[AllowAnonymous]
        //[HttpGet]
        //public async Task<ActionResult<List<AppUser>>> getusers()
        //{
        //    return await _context.Users.ToListAsync();
        //}

    }
}
