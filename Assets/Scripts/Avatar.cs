using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAlice;

public class Avatar : MonoBehaviour
{
    public int ID;
    public SIKBVHData sikBVHData;


    private Transform[] boneMaps = new Transform[59];

    // Start is called before the first frame update
    void Start()
    {
        InitBoneBinding();
    }

    void InitBoneBinding()
    {
        var PN_Avatar = this.transform.GetChild(0);
        var hips = PN_Avatar.transform.GetChild(0);

        // bind bones
        TraverseBones(hips);
    }

    void TraverseBones(Transform bone)
    {
        // get bone name
        var name = SIKHelper.MapBoneName(bone.name);
        if (name == "") return;

        // get bone index
        int boneIndex = (int)ParseEnum<SIKHelper.SikBones>(name);

        // bind data
        boneMaps[boneIndex] = bone;

        // bind children
        for (int i = 0; i < bone.childCount; i++)
        {
            var tsc = bone.GetChild(i);
            TraverseBones(tsc);
        }
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)SIKHelper.SikBones.Parse(typeof(T), value, true);
    }

    // Update is called once per frame
    void Update()
    {
        boneMaps[0].localRotation = AliceDataReader.FlipBVHHipRotation(sikBVHData.rootData[0], sikBVHData.rootData[1], sikBVHData.rootData[2], sikBVHData.rootData[3]);
        boneMaps[0].localPosition = AliceDataReader.FlipBVHHipPosition(sikBVHData.rootData[4], sikBVHData.rootData[5], sikBVHData.rootData[6]);
        boneMaps[0].localScale = new Vector3(sikBVHData.scale, sikBVHData.scale, sikBVHData.scale);

        int boneDataCnt = sikBVHData.bWithDisp == 1 ? 7 : 4;
        int boneCnt = (int)SIKHelper.SikBones.MaxBone - 1;
        for (int i = 0; i < boneCnt; i++)
        {
            int offset = boneDataCnt * i;
            if (sikBVHData.bWithDisp == 1)
            {
                boneMaps[i + 1].localRotation = AliceDataReader.FlipBVHRotation(sikBVHData.data[offset], sikBVHData.data[offset + 1], sikBVHData.data[offset + 2], sikBVHData.data[offset + 3]);
                boneMaps[i + 1].localPosition = AliceDataReader.FlipBVHPosition(sikBVHData.data[offset + 4], sikBVHData.data[offset + 5], sikBVHData.data[offset + 6]);
            }
            else
            {
                boneMaps[i + 1].localRotation = AliceDataReader.FlipBVHRotation(sikBVHData.data[offset], sikBVHData.data[offset + 1], sikBVHData.data[offset + 2], sikBVHData.data[offset + 3]);
            }
        }
    }
}
