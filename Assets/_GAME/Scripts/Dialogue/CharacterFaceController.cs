using System.Collections;
using UnityEditor;
using UnityEngine;

public class CharacterFaceController : MonoBehaviour
{
    [SerializeField] BonesReference bonesRef;

    [SerializeField] Vector3 upperLidLeft_V3_default, upperLidLeft_V3_blink;
    [SerializeField] Vector3 lowerLidLeft_V3_default, lowerLidLeft_V3_blink;
    [SerializeField] Vector3 upperLidRight_V3_default, upperLidRight_V3_blink;
    [SerializeField] Vector3 lowerLidRight_V3_default, lowerLidRight_V3_blink;

    [SerializeField] Vector3 browInnerLeft_V3_default, browOuterLeft_V3_default, browInnerRight_V3_default, browOuterRight_V3_default;
    [SerializeField] Vector3 browInnerLeft_V3_angry, browOuterLeft_V3_angry, browInnerRight_V3_angry, browOuterRight_V3_angry;
    [SerializeField] Vector3 browInnerLeft_V3_fear, browOuterLeft_V3_fear, browInnerRight_V3_fear, browOuterRight_V3_fear;

    public enum Expression
    {
        Neutral,
        Angry,
        Fear,
    }

    [SerializeField] Expression currentExpression;

    private void Start()
    {
        StartCoroutine(blink());
        ChangeExpression(currentExpression);
    }

    IEnumerator blink()
    {
        yield return new WaitForSeconds(Random.Range(3f, 4f));
        float blinkSpeed = Random.Range(0.1f, 0.4f);
        float halfBlink = blinkSpeed / 2f;

        // Close eyes
        yield return StartCoroutine(LerpLids(
            bonesRef.upperLidLeft.localPosition, upperLidLeft_V3_blink,
            bonesRef.lowerLidLeft.localPosition, lowerLidLeft_V3_blink,
            bonesRef.upperLidRight.localPosition, upperLidRight_V3_blink,
            bonesRef.lowerLidRight.localPosition, lowerLidRight_V3_blink,
            halfBlink
        ));

        // Open eyes
        yield return StartCoroutine(LerpLids(
            bonesRef.upperLidLeft.localPosition, upperLidLeft_V3_default,
            bonesRef.lowerLidLeft.localPosition, lowerLidLeft_V3_default,
            bonesRef.upperLidRight.localPosition, upperLidRight_V3_default,
            bonesRef.lowerLidRight.localPosition, lowerLidRight_V3_default,
            halfBlink
        ));

        StartCoroutine(blink());
    }

    IEnumerator LerpLids(
        Vector3 ulStart, Vector3 ulEnd,
        Vector3 llStart, Vector3 llEnd,
        Vector3 urStart, Vector3 urEnd,
        Vector3 lrStart, Vector3 lrEnd,
        float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            bonesRef.upperLidLeft.localPosition = Vector3.Lerp(ulStart, ulEnd, t);
            bonesRef.lowerLidLeft.localPosition = Vector3.Lerp(llStart, llEnd, t);
            bonesRef.upperLidRight.localPosition = Vector3.Lerp(urStart, urEnd, t);
            bonesRef.lowerLidRight.localPosition = Vector3.Lerp(lrStart, lrEnd, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        bonesRef.upperLidLeft.localPosition = ulEnd;
        bonesRef.lowerLidLeft.localPosition = llEnd;
        bonesRef.upperLidRight.localPosition = urEnd;
        bonesRef.lowerLidRight.localPosition = lrEnd;
    }

    public void ChangeExpression(Expression newExpression)
    {
        currentExpression = newExpression;

        Vector3 bIL_Target = browInnerLeft_V3_default;
        Vector3 bOL_Target = browOuterLeft_V3_default;
        Vector3 bIR_Target = browInnerRight_V3_default;
        Vector3 bOR_Target = browOuterRight_V3_default;

        if (newExpression == Expression.Angry)
        {
            bIL_Target = browInnerLeft_V3_angry;
            bOL_Target = browOuterLeft_V3_angry;
            bIR_Target = browInnerRight_V3_angry;
            bOR_Target = browOuterRight_V3_angry;
        }

        if (newExpression == Expression.Fear)
        {
            bIL_Target = browInnerLeft_V3_fear;
            bOL_Target = browOuterLeft_V3_fear;
            bIR_Target = browInnerRight_V3_fear;
            bOR_Target = browOuterRight_V3_fear;
        }

        StopAllCoroutines(); // Stops any previous animations like blinking (optional)
        StartCoroutine(LerpBrows(
            bonesRef.browInnerLeft.localPosition, bIL_Target,
            bonesRef.browOuterLeft.localPosition, bOL_Target,
            bonesRef.browInnerRight.localPosition, bIR_Target,
            bonesRef.browOuterRight.localPosition, bOR_Target,
            0.25f // duration of brow transition
        ));

        // You may want to restart blink after expression settles:
        StartCoroutine(blink());
    }

    IEnumerator LerpBrows(
        Vector3 bIL_Start, Vector3 bIL_End,
        Vector3 bOL_Start, Vector3 bOL_End,
        Vector3 bIR_Start, Vector3 bIR_End,
        Vector3 bOR_Start, Vector3 bOR_End,
        float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            bonesRef.browInnerLeft.localPosition = Vector3.Lerp(bIL_Start, bIL_End, t);
            bonesRef.browOuterLeft.localPosition = Vector3.Lerp(bOL_Start, bOL_End, t);
            bonesRef.browInnerRight.localPosition = Vector3.Lerp(bIR_Start, bIR_End, t);
            bonesRef.browOuterRight.localPosition = Vector3.Lerp(bOR_Start, bOR_End, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        bonesRef.browInnerLeft.localPosition = bIL_End;
        bonesRef.browOuterLeft.localPosition = bOL_End;
        bonesRef.browInnerRight.localPosition = bIR_End;
        bonesRef.browOuterRight.localPosition = bOR_End;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterFaceController))]
public class CharacterFaceControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterFaceController controller = (CharacterFaceController)target;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Expression Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Set Neutral Expression"))
        {
            controller.ChangeExpression(CharacterFaceController.Expression.Neutral);
        }

        if (GUILayout.Button("Set Angry Expression"))
        {
            controller.ChangeExpression(CharacterFaceController.Expression.Angry);
        }

        if (GUILayout.Button("Set Fear Expression"))
        {
            controller.ChangeExpression(CharacterFaceController.Expression.Fear);
        }
    }
}
#endif