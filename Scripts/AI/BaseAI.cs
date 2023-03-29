using Godot;
using System;
using Godot.Collections;

public abstract class BaseAI : Node
{
    protected Humanoid human;
    protected FightScene fightScene;
    protected Humanoid enemy;
    protected MusicHandler mh;
    protected SongData songData;
    protected bool regenning = false;

    protected float regenThresholdPerc = 0.8f;
    [Export]
    public bool On = true;
    protected int phase = 0;
    public abstract void onBeat(int beat);

    public void onReady()
    {
        if (!On) return;
        human = GetParentOrNull<Humanoid>();
        if (human == null) return;
        
        if (!human.atkMode)
        {
            QueueFree();
            return;
        }
        fightScene = human.GetParentOrNull<FightScene>();
        if(fightScene == null) return;

        enemy = fightScene.player1Instance;
        mh = GlobalHandler.CurrentMusic;
        songData = mh.songData;
        mh.Connect("EmitBeat", this, "onBeat");
    }
    public Vector2 EnemyPosition()
    {
        return enemy.Position;

    }
    public Vector2 CastToGrid(Vector2 pos)
    {
        float x = pos.x - (pos.x % 16) + 8;
        float y = pos.y - (pos.y % 16) + 8;
        return new Vector2(x, y);
    }
    public Vector2 ToEnemy()
    {
        Vector2 dist = CastToGrid(EnemyPosition() - human.Position);
        float length = dist.Length();
        if (length <=32.0f) return Vector2.Zero;
        int ySign = Mathf.Sign(dist.y);
        int xSign = Mathf.Sign(dist.x);
        if (Mathf.Abs(dist.y)> Mathf.Abs(dist.x))
        {
            return new Vector2(0, ySign*16);
        }
        else
        {
            return new Vector2(xSign*16, 0);
        }

    }
    public Vector2 AwayFromEnemy()
    {
        Vector2 dist = CastToGrid(EnemyPosition() - human.Position);
        int ySign = Mathf.Sign(dist.y);
        int xSign = Mathf.Sign(dist.x);
        if (Mathf.Abs(dist.y) > Mathf.Abs(dist.x))
        {
            return new Vector2(-xSign * 16, 0);
        }
        else
        {
            return new Vector2(0, -ySign * 16);
        }

    }
    public Vector2 ToBackofEnemy()
    {
        Vector2 targetPosition = CastToGrid(EnemyPosition());
        if(enemy.facingLeft)
        {
            targetPosition.x += 16;
        }
        else
        {
            targetPosition.x -= 16;
        }
        return targetPosition;
    }
    public bool canMove(int beat)
    {
        if (!songData.beatsToMove.Contains(beat%mh.songTimeSig)) return false;
        else return true;
    }
    public void Teleport(Vector2 position)
    {
        human.Position = position;
    }
    /// <summary>
    /// Returns true if still regenning
    /// </summary>
    /// <returns></returns>
    public bool RegenPhase()
    {
        int insp = human.GetInspirationCount();
        float maxInsp = human.GetMaxInspirationCount();
        if(insp <= maxInsp * regenThresholdPerc)
        {
            human.RegenInspiration();
            return true;
        }
        else
        {
            return false;
        }

    }
    public void TryAttack()
    {
        if (human.Attack(EnemyPosition()) == false)
        {
            regenning = true;
        };
    }
}
