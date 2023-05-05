using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Normal,
    Obstacle,
    Breakable
}

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    
    Board m_board;
    SpriteRenderer m_spriteRenderer;

    public TileType tileType = TileType.Normal;

    
    public Color m_normalColor;

    public int breakableValue;
    public Sprite[] breakableSprites;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int x, int y, Board board)
    {
        xIndex = x;
        yIndex = y;
        m_board = board;
        
        if(tileType == TileType.Breakable)
        {
            if (breakableSprites[breakableValue] != null)
            {
                m_spriteRenderer.sprite = breakableSprites[breakableValue];
            }
        }
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        if (m_board != null)
        {
            m_board.ClickTile(this);
        }
    }

    /// <summary>
    /// Called when the mouse enters the GUIElement or Collider.
    /// </summary>
    void OnMouseEnter()
    {
        if (m_board != null)
        {
            m_board.DragToTile(this);
        }
    }

    /// <summary>
    /// OnMouseUp is called when the user has released the mouse button.
    /// </summary>
    void OnMouseUp()
    {
        if (m_board != null)
        {
            m_board.ReleaseTile();
        }
    }

    public void BreakTile()
    {
        if (tileType != TileType.Breakable)
        {
            return;
        }

        StartCoroutine(BreakTileRoutine());
    }

    IEnumerator BreakTileRoutine()
    {
        breakableValue--;
        breakableValue = Mathf.Clamp(breakableValue, 0, breakableValue);

        yield return new WaitForSeconds(0.25f);

        if (breakableSprites[breakableValue] != null)
        {
            m_spriteRenderer.sprite = breakableSprites[breakableValue];
        }

        if (breakableValue <= 0)
        {
            tileType = TileType.Normal;
            m_spriteRenderer.color = m_normalColor;
        }
    }
}
