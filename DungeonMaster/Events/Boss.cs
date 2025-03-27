using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Events
{
    public class Boss : IEvent
    {
        public string Type => throw new NotImplementedException();
        public List<string> Description { get; set; }

        public void BeforeNextRoom()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void SetDefaultOptions()
        {
            throw new NotImplementedException();
        }

        public void SetUIState()
        {
            throw new NotImplementedException();
        }
    }
}
