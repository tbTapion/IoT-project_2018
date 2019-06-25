using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameTile
{
    void SetActive();
    void SetOtherTileList(List<IGameTile> tileList);
}
