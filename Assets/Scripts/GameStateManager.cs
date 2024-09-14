
public class GameStateManager
{
    public int turn = 0;
    public int playerScore = 0;
    public int nextGoodieBagScore = 2;
    
    void endTurn()
    {
        turn++;
        updateScore();
        levelUpTiles();
        tryGiveGoodieBag();
    }
    
    void updateScore()
    {
        playerScore += 1;
        //TODO Implement
    }
    
    void levelUpTiles()
    {
        //TODO Implement
    }
    
    void tryGiveGoodieBag()
    {
        if (playerScore >= nextGoodieBagScore)
        {
            //giveGoodieBag(); // Maybe pass score as param
            
            // Calculate next goodie bag score
            nextGoodieBagScore = (nextGoodieBagScore + 2) * 2;
        }
    }

}