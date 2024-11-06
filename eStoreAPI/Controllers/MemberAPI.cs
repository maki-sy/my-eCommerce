using AutoMapper;
using BusinessObject.Models;
using eStoreAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberAPI : ControllerBase
    {
        private IMemberRepository repo = new MemberRepository();
        private readonly IMapper _mapper;

        public MemberAPI(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/<MemberAPI>
        [HttpGet]
        public ActionResult<IEnumerable<Member>> GetMembers() => repo.GetMembers();

        [HttpGet("getbyid")]
        public ActionResult<MemberDTO> GetMemberById(int id)
        {
            Member member = repo.GetMemberById(id);
            if (member == null)
            { 
                return BadRequest("Not Found!");
            }
            MemberDTO dto = _mapper.Map<MemberDTO>(member);
            return dto;
        }

        // POST api/<MemberAPI>
        [HttpPost]
        public IActionResult PostMember(MemberDTO mDTO)
        {
            if (mDTO == null)
            {
                return BadRequest("Member data is null.");
            }
            Member m = _mapper.Map<Member>(mDTO);
            repo.SaveMember(m);
            return NoContent();
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDTO credentials)
        {
            using (var context = new PRN231_AS1Context())
            {
                if (credentials.Email == null || credentials.Password == null)
                {
                    return BadRequest("Login credentials is null.");
                }
                else
                {
                    Member m = context.Members.Where(x=>x.Email == credentials.Email).FirstOrDefault();
                    if (m == null)
                    {
                        return BadRequest("Invalid credentials");
                    }
                    else
                    {
                        if (m.Password != credentials.Password)
                        {
                            return Unauthorized("Username or password is incorrect.");
                        }
                        else
                        {
                            return Ok("Login success!");
                        }
                    }
                }
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateMember(int id, MemberDTO mDTO)
        {
            if (mDTO == null)
            {
                return BadRequest("Member data is null.");
            }
            Member m = _mapper.Map<Member>(mDTO);
            m.MemberId = id;
            repo.UpdateMember(m);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteMember(int id)
        {
            Member m = new Member { MemberId = id };
            repo.DeleteMember(m);
            return NoContent();
        }
    }
}
