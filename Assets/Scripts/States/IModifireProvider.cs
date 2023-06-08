using System.Collections.Generic;
using System;
namespace RPG.States
{
    interface IModifireProvider
    {
        IEnumerable<float> GetAddtiveModifires(State state); 
        IEnumerable<float> GetPercentagesModifires(State state); 

    }

}