using Server.Network;
using Server.Room.Componets;
using Server.Room.Interface;

namespace Server.Room.Example;

public class UnitSelection
{
    public Player SelectPlayer(ushort character)
    {
        Unit unit;
        
        switch (character)
        {
            case 1:
                unit = new SpongeBob(); break;
            
            default:
                
                unit = new SpongeBob(); break;
        }

        return (Player)unit;
    }
    
    public Enemy SelectEnemy(int character)
    {
        Unit unit;
        
        switch (character)
        {
            case 1:
                
                unit = new SpongeBob(); break;
            
            default:
              
                unit = new SpongeBob(); break;
        }

        return (Enemy)unit;
    }
}