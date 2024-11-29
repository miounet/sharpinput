using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [Serializable]
    public class RequestEntity
    {
        public RequestEntity()
        {

        }
        // Fields
        public string Content;
        public string OptCommand;

        // Methods
    
    }



    [Serializable]
    public class CheckEntity
    {
        // Fields
        public int CheckFlag;
        public string HVID;
        public string ProVer;
        public DateTime? SpanTime;

        // Methods
        public CheckEntity() { }
    }
    [Serializable]
    public class UpdateDictEnt
    {
        // Fields
        public string Dictid;
        public string DictName;
        public string DictVersion;
        public string ProductVer;
        public bool Select;
        public short UpdateFalg;
        public List<string> UpdateUrl;

        // Methods
        public UpdateDictEnt() { }
    }
    public enum OptCom
    {
        GetUpdateDict,
        StartUpdateDict,
        EndUpdateDict,
        GetCloudDict,
        GetMyInfo,
        GetProDict,
        CheckLic,
        StudentLogin,
        GetStudentOpt,
        UploadStudentScore,
        GetStudentByTeacher,
        AddKHOpt,
        UpdateUserPwd
    }
    [Serializable]
    public class ColdDictEntity
    {
        // Fields
     
        public bool NextHave;
  
        public int Pos;
      
        public List<string> Value;

        // Methods
        public ColdDictEntity() { }

  

    }


    [Serializable]
    public class ProDictEntity
    {
        // Fields
        public List<UpdateDictEnt> DictList;
        public string HVID;
        public string SearchKey;

        // Methods
        public ProDictEntity() { }
    }



}
