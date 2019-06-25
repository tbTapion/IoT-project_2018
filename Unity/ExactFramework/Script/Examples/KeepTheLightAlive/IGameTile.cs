using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeepAliveExample
{
    public interface IGameTile
    {
        void SetActive();
        void SetOtherTiles(List<IGameTile> list);
    }
}
