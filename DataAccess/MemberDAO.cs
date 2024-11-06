using BusinessObject.Models;

namespace DataAccess
{
    public class MemberDAO
    {
        public static List<Member> GetMembers()
        {
            var listM = new List<Member>();
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    listM = context.Members.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listM;
        }
        public static Member FindMemberById(int id)
        {
            Member m = new Member();
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    m = context.Members.FirstOrDefault(pro => pro.MemberId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return m;
        }
        public static void SaveMember(Member m)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    context.Members.Add(m);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void UpdateMember(Member m)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    context.Entry<Member>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public static void DeleteMember(Member m)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    var m1 = context.Members.SingleOrDefault(pro => pro.MemberId == m.MemberId);

                    context.Members.Remove(m1);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
