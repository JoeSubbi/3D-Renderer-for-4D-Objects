public class ShapeGenerator {

    public static void unit3Cube(){
        Vector3[] coords = new Vector3[]{
                new Vector3(-1,-1,-1), new Vector3(1,-1,-1), new Vector3(1,1,-1),new Vector3(-1,1,-1),
                new Vector3(-1,-1,1), new Vector3(1,-1,1), new Vector3(1,1,1), new Vector3(-1,1,1)
        };
        Vector3[][] faces = new Vector3[][]{
                {coords[4],coords[5],coords[6],coords[7]},//top (+z)
                {coords[0],coords[1],coords[2],coords[3]},//bottom (-z)
                {coords[1],coords[2],coords[5],coords[6]},//north (+x)
                {coords[0],coords[1],coords[4],coords[5]},//east (-y)
                {coords[3],coords[0],coords[4],coords[7]},//south (-x)
                {coords[2],coords[3],coords[7],coords[6]},//west (+y)
        };

        ThreeObject unitCube = new ThreeObject(coords, faces);
    }

    public static void unit4Cube(){
        Vector4[] coords = new Vector4[]{
                //"Outer" "3D" Cube
                new Vector4(-1,-1,-1,-1), new Vector4(1,-1,-1,-1), new Vector4(1,1,-1,-1),new Vector4(-1,1,-1,-1),
                new Vector4(-1,-1,1,-1), new Vector4(1,-1,1,-1), new Vector4(1,1,1,-1), new Vector4(-1,1,1,-1),
                //"Inner" "4D" Cube
                new Vector4(-1,-1,-1,1), new Vector4(1,-1,-1,1), new Vector4(1,1,-1,1),new Vector4(-1,1,-1,1),
                new Vector4(-1,-1,1,1), new Vector4(1,-1,1,1), new Vector4(1,1,1,1), new Vector4(-1,1,1,1)

        };
        Vector4[][] faces = new Vector4[][]{
                //"Outer" "3D" Cube (-w)
                {coords[4],coords[5],coords[6],coords[7]},//top (+z)
                {coords[0],coords[1],coords[2],coords[3]},//bottom (-z)
                {coords[1],coords[2],coords[5],coords[6]},//north (+x)
                {coords[0],coords[1],coords[4],coords[5]},//east (-y)
                {coords[3],coords[0],coords[4],coords[7]},//south (-x)
                {coords[2],coords[3],coords[7],coords[6]},//west (+y)
                //"Inner" "4D" Cube (+w)
                {coords[12],coords[13],coords[14],coords[15]},//top (+z)
                {coords[8],coords[9],coords[10],coords[11]},//bottom (-z)
                {coords[9],coords[10],coords[13],coords[14]},//north (+x)
                {coords[8],coords[9],coords[12],coords[13]},//east (-y)
                {coords[11],coords[8],coords[12],coords[15]},//south (-x)
                {coords[10],coords[11],coords[15],coords[14]},//west (+y)
                //Joining Faces
                {coords[13],coords[14],coords[6],coords[5]},//top north
                {coords[12],coords[13],coords[5],coords[4]},//top east
                {coords[15],coords[12],coords[4],coords[7]},//top south
                {coords[14],coords[15],coords[7],coords[6]},//top west

                {coords[9],coords[10],coords[2],coords[1]},//bottom north
                {coords[8],coords[9],coords[1],coords[0]},//bottom east
                {coords[11],coords[8],coords[0],coords[3]},//bottom south
                {coords[10],coords[11],coords[3],coords[2]},//bottom west

                {coords[13],coords[9],coords[1],coords[5]},//north east
                {coords[12],coords[8],coords[0],coords[4]},//east south
                {coords[15],coords[11],coords[3],coords[7]},//south west
                {coords[14],coords[11],coords[2],coords[6]},//west north
        };

        FourObject unitCube = new FourObject(coords, faces);
    }
}
