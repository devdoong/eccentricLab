using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 셀을 나타내는 클래스. 셀은 GameObject들의 집합을 관리합니다.
class Cell
{
    // 해당 셀에 포함된 GameObject들을 저장하는 HashSet.
    public HashSet<GameObject> Objects { get; } = new HashSet<GameObject>();
}

// 그리드를 제어하는 컨트롤러 클래스. BaseController를 상속합니다.
public class GridController : BaseController
{
    // Unity의 Grid 컴포넌트를 저장할 필드.
    UnityEngine.Grid _grid;

    // 그리드 상의 셀들을 위치(Vector3Int)와 함께 관리하는 딕셔너리.
    Dictionary<Vector3Int, Cell> _cells = new Dictionary<Vector3Int, Cell>();

    // 초기화 메서드. Grid 컴포넌트를 가져오거나 추가합니다.
    public override bool Init()
    {
        base.Init(); // 부모 클래스의 Init 메서드 호출.

        // Grid 컴포넌트가 없으면 추가(GetOrAddComponent 사용).
        _grid = gameObject.GetOrAddComponent<UnityEngine.Grid>();

        return true; // 초기화 성공을 반환.
    }

    // 게임 오브젝트를 그리드 셀에 추가하는 메서드.
    public void Add(GameObject go)
    {
        // 오브젝트의 월드 좌표를 셀 좌표로 변환.
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        // 해당 좌표의 셀을 가져옵니다.
        Cell cell = GetCell(cellPos);
        if (cell == null) // 셀이 없으면 종료.
            return;

        // 셀에 오브젝트를 추가합니다.
        cell.Objects.Add(go);
    }

    // 게임 오브젝트를 그리드 셀에서 제거하는 메서드.
    public void Remove(GameObject go)
    {
        // 오브젝트의 월드 좌표를 셀 좌표로 변환.
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        // 해당 좌표의 셀을 가져옵니다.
        Cell cell = GetCell(cellPos);
        if (cell == null) // 셀이 없으면 종료.
            return;

        // 셀에서 오브젝트를 제거합니다.
        cell.Objects.Remove(go);
    }

    // 지정된 좌표에 해당하는 셀을 가져오거나, 없으면 새로 만듭니다.
    Cell GetCell(Vector3Int cellPos)
    {
        Cell cell = null;

        // 딕셔너리에서 셀을 찾습니다. 없으면 새로 생성합니다.
        if (_cells.TryGetValue(cellPos, out cell) == false)
        {
            cell = new Cell(); // 새로운 셀 생성.
            _cells.Add(cellPos, cell); // 딕셔너리에 추가.
        }

        return cell; // 셀 반환.
    }

    // 주어진 위치(pos)에서 특정 범위(range) 내에 있는 오브젝트들을 수집하는 메서드.
    public List<GameObject> GatherObjects(Vector3 pos, float range)
    {
        // 수집된 오브젝트들을 담을 리스트.
        List<GameObject> objects = new List<GameObject>();

        // 범위의 좌우, 상하 경계를 셀 좌표로 변환.
        Vector3Int left = _grid.WorldToCell(pos + new Vector3(-range, 0));
        Vector3Int right = _grid.WorldToCell(pos + new Vector3(+range, 0));
        Vector3Int bottom = _grid.WorldToCell(pos + new Vector3(0, -range));
        Vector3Int top = _grid.WorldToCell(pos + new Vector3(0, +range));

        // 각 경계의 최소 및 최대 값을 설정합니다.
        int minX = left.x;
        int maxX = right.x;
        int minY = bottom.y;
        int maxY = top.y;

        // X와 Y의 범위 내 모든 셀을 순회합니다.
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                // 해당 좌표에 셀이 없으면 건너뜁니다.
                if (_cells.ContainsKey(new Vector3Int(x, y, 0)) == false)
                    continue;

                // 셀에 있는 모든 오브젝트를 리스트에 추가합니다.
                objects.AddRange(_cells[new Vector3Int(x, y, 0)].Objects);
            }
        }

        return objects; // 수집된 오브젝트 리스트 반환.
    }
}
