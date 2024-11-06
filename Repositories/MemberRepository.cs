using BusinessObject.Models;
using DataAccess;

namespace Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public void DeleteMember(Member p) => MemberDAO.DeleteMember(p);

        public Member GetMemberById(int id) => MemberDAO.FindMemberById(id);

        public List<Member> GetMembers() => MemberDAO.GetMembers();


        public void SaveMember(Member p) => MemberDAO.SaveMember(p);


        public void UpdateMember(Member p) => MemberDAO.UpdateMember(p);

    }
}
