using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Game.Model.Items
{
    internal enum Situations
    {
        MeetZavrCanInteractIHaveMoreEnergiISrtonge,
        MeetZavrCanInteractIHaveMoreEnergiWeSame,
        MeetZavrCanInteractIHaveMoreEnergiIWeaker,

        MeetZavrCanInteractIHaventMoreEnergiISrtonge,
        MeetZavrCanInteractIHaventMoreEnergiWeSame,
        MeetZavrCanInteractIHaventMoreEnergiIWeaker,

        MeetZavrCantInteractIHaveMoreEnergiISrtonge,
        MeetZavrCantInteractIHaveMoreEnergiWeSame,
        MeetZavrCantInteractIHaveMoreEnergiIWeaker,

        MeetZavrCantInteractIHaventMoreEnergiISrtonge,
        MeetZavrCantInteractIHaventMoreEnergiWeSame,
        MeetZavrCantInteractIHaventMoreEnergiIWeaker,



        MeetTreeCanInteractIHaveMoreEnergiISrtonge,
        MeetTreeCanInteractIHaveMoreEnergiIWeaker,

        MeetTreeCanInteractIHaventMoreEnergiISrtonge,
        MeetTreeCanInteractIHaventMoreEnergiIWeaker,

        MeetTreeCantInteractIHaveMoreEnergiISrtonge,
        MeetTreeCantInteractIHaveMoreEnergiIWeaker,

        MeetTreeCantInteractIHaventMoreEnergiISrtonge,
        MeetTreeCantInteractIHaventMoreEnergiIWeaker,

        MeetRockCanInteract,
        MeetRockCantInteract,

        MeetNothing,
    }
}
