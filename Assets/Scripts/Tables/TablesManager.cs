using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class TablesManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<Table> _tables = new List<Table>();

    private Queue<ClientBrain> _clientsToAssign = new Queue<ClientBrain>();

    public void RequestTable(ClientBrain client) => _clientsToAssign.Enqueue(client);
    public void ReleaseTable(Table table) => table.IsReserved = false;
    public Table GetFreeTable()
    {
        var tables = _tables.Where((table) => 
                        !table.IsReserved &&
                        table.HasSpace())
                        .ToList();
        
        if (tables.Count == 0) return null;
                        
        return tables[Random.Range(0, tables.Count)];
    }


    private void ResolveOldestRequest()
    {
        ClientBrain client = _clientsToAssign.Dequeue();

        Table table = GetFreeTable();

        if (table == null)
        {
            client.NoTables();
            return;
        }

        table.IsReserved = true;

        client.SetTable(table);
    }

    public void Start()
    {
        _tables = GetComponentsInChildren<Table>().ToList();
    }

    private void Update()
    {
        if (_clientsToAssign.Count > 0)
        {
            ResolveOldestRequest();
        }
    }
}
