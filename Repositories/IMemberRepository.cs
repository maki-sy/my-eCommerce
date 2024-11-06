using BusinessObject.Models;

namespace Repositories
{
    public interface IMemberRepository
    {
        void SaveMember(Member p);
        Member GetMemberById(int id);
        void DeleteMember(Member p);
        void UpdateMember(Member p);
        List<Member> GetMembers();
    }
}
