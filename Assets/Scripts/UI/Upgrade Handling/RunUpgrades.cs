using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunUpgrades : MonoBehaviour
{

    private List<int> upgradeAmount= new List<int>();
    private List<int> upgradeNames = new List<int>();
    void Awake()
    {
        //get meta upgrades, what upgrades are changed. 
    }


    /*
    cool stack overflow answer, could implement this as a separate function if I want to. No point whatsoever as the stack of upgrades will only
    ever be at most 10 just might be cool.

    You can achieve constant amortized time per operation by keeping a dynamically-sized array A
    (using the doubling/halving technique). To insert an element append it at the end. To implement remove_random() generate a random index k
    between 1 and n, swap A[k] with A[n] and delete (and return) A[n].

    If you want a non-amortized worst-case bound on the time complexity, then an AVL in which each node v
    has been augmented to also store the size of the subtree rooted in v
    supports both those operation in O(logn) worst-case time per operation.

    To implement remove_random() simply generate a random number k between 1 and n and find the element e of rank k
    in the tree. Then delete e from the tree and return it.
    */

    public void ChooseRandomUpgrades(int number) {
        for (int i = 0; i < number; i++) {
            //choose random upgrade from list of upgrades
        }
    }
}
