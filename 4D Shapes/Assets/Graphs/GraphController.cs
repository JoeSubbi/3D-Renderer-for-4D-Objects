using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour
{
    public GameObject OptionResults;
    public GameObject PoseResults;

    // Start is called before the first frame update
    void Awake()
    {
        // Read JSON file

        // set height of all result columns to 0
        // dictionary of all result columns

        // loop through each node[rep]
            // loop through node[rep][shape_match]
                // get node[test_id]
                // if shape == shape
                    // result columns[rep][shape_match] height +=  1 (33)
            // loop through node[rep][rotation_match]
                // get node[test_id]
                // loop through each loaded rotation
                    // if equal to selected rotation
                        //result columns[rep][rotation_match] height +=  (1/6) (5.5)
            // loop through node[rep][pose_match]
                // get node[test_id]
                // result columns[rep][pose_match] height += accuracy percentage
            // result columns[rep][pose_match] height /= MAX_TESTS
    }

}
