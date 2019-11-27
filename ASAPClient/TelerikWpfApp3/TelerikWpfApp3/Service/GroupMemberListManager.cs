using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.Service
{
    public class GroupMemberListManager
    {
        private IDictionary<string, List<string>> groupMemberList //그룹 인덱스 와 멤버들
            = new Dictionary<string, List<string>>();
        public void AddGroupMemberList(string gIdx, List<string> groupMemberList)
        {
            this.groupMemberList.Add(gIdx, groupMemberList);
        }
        public List<string> getGroupMemberList(string gIdx)
        {
            return groupMemberList[gIdx];
        }
    }
}
