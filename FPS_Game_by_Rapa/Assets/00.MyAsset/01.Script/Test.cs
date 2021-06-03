using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    char[] letters = new char[3];

    void Start()
    {
        letters[0] = '박';
        letters[1] = '원';
        letters[2] = '석';

        string test = new string(letters);

        print(test);
    }
}
