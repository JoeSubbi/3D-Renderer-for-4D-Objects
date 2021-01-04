public class ShapeGenerator {

    public static ThreeObject ThreeCube(double minor, double major, double xOffset, double yOffset, double zOffset){
        /* Unit 3Cube
        Vector4[] coords = new Vector4[]{
                new Vector4(-1,-1,-1), new Vector4(1,-1,-1), new Vector4(1,1,-1),new Vector4(-1,1,-1),
                new Vector4(-1,-1,1), new Vector4(1,-1,1), new Vector4(1,1,1), new Vector4(-1,1,1),
        };*/

        Vector3[] coords = new Vector3[]{
                //Bottom Square
                new Vector3(minor+xOffset,minor+yOffset,minor+zOffset),
                new Vector3(major+xOffset,minor+yOffset,minor+zOffset),
                new Vector3(major+xOffset,major+yOffset,minor+zOffset),
                new Vector3(minor+xOffset,major+yOffset,minor+zOffset),
                //Top Square
                new Vector3(minor+xOffset,minor+yOffset,major+zOffset),
                new Vector3(major+xOffset,minor+yOffset,major+zOffset),
                new Vector3(major+xOffset,major+yOffset,major+zOffset),
                new Vector3(minor+xOffset,major+yOffset,major+zOffset)
        };
        Vector3[][] faces = new Vector3[][]{
                {coords[4],coords[5],coords[6],coords[7]},//top (+z)
                {coords[0],coords[1],coords[2],coords[3]},//bottom (-z)
                {coords[1],coords[2],coords[5],coords[6]},//north (+x)
                {coords[0],coords[1],coords[4],coords[5]},//east (-y)
                {coords[3],coords[0],coords[4],coords[7]},//south (-x)
                {coords[2],coords[3],coords[7],coords[6]},//west (+y)
        };
        return new ThreeObject(coords, faces);
    }

    public static FourObject FourCube(double minor, double major, double xOffset, double yOffset, double zOffset, double wOffset){
        /* Unit 4Cube
        Vector4[] coords = new Vector4[]{
                //"Outer" "3D" Cube
                new Vector4(-1,-1,-1,-1), new Vector4(1,-1,-1,-1), new Vector4(1,1,-1,-1),new Vector4(-1,1,-1,-1),
                new Vector4(-1,-1,1,-1), new Vector4(1,-1,1,-1), new Vector4(1,1,1,-1), new Vector4(-1,1,1,-1),
                //"Inner" "4D" Cube
                new Vector4(-1,-1,-1,1), new Vector4(1,-1,-1,1), new Vector4(1,1,-1,1),new Vector4(-1,1,-1,1),
                new Vector4(-1,-1,1,1), new Vector4(1,-1,1,1), new Vector4(1,1,1,1), new Vector4(-1,1,1,1)
        };*/

        Vector4[] coords = new Vector4[]{
                //"Outer" "3D" Cube
                //Bottom Square
                new Vector4(minor+xOffset,minor+yOffset,minor+zOffset, minor+wOffset),
                new Vector4(major+xOffset,minor+yOffset,minor+zOffset, minor+wOffset),
                new Vector4(major+xOffset,major+yOffset,minor+zOffset, minor+wOffset),
                new Vector4(minor+xOffset,major+yOffset,minor+zOffset, minor+wOffset),
                //Top Square
                new Vector4(minor+xOffset,minor+yOffset,major+zOffset, minor+wOffset),
                new Vector4(major+xOffset,minor+yOffset,major+zOffset, minor+wOffset),
                new Vector4(major+xOffset,major+yOffset,major+zOffset, minor+wOffset),
                new Vector4(minor+xOffset,major+yOffset,major+zOffset, minor+wOffset),
                //"Inner" "4D" Cube
                //Bottom Square
                new Vector4(minor+xOffset,minor+yOffset,minor+zOffset, major+wOffset),
                new Vector4(major+xOffset,minor+yOffset,minor+zOffset, major+wOffset),
                new Vector4(major+xOffset,major+yOffset,minor+zOffset, major+wOffset),
                new Vector4(minor+xOffset,major+yOffset,minor+zOffset, major+wOffset),
                //Top Square
                new Vector4(minor+xOffset,minor+yOffset,major+zOffset, major+wOffset),
                new Vector4(major+xOffset,minor+yOffset,major+zOffset, major+wOffset),
                new Vector4(major+xOffset,major+yOffset,major+zOffset, major+wOffset),
                new Vector4(minor+xOffset,major+yOffset,major+zOffset, major+wOffset)
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

        return new FourObject(coords, faces);
    }

    public static double getMajorCoord(double size){
        return size/2;
    }

    public static void unit3Cube(){
        ThreeCube(-1,1,0,0,0);
    }

    public static void FourByFourPositiveZ3Cube(){
        // bottom corner is (-2,-2,-2), top corner is (2,2,2)
        // with z offset of 2 the bottom corner will be (-2,-2,0), top corner will be (2,2,4)
        ThreeCube(-2,2,0,0,2);
    }

    public static void unit4Cube(){
        FourCube(-1,1,0,0,0, 0);
    }

    public static void FourByFourPositiveZW4Cube(){
        // bottom corner is (-2,-2,-2,-2), top corner is (2,2,2,2)
        // with zw offset of 2 the bottom corner will be (-2,-2,0,0), top corner will be (2,2,4,4)
        FourCube(-2,2,0,0,2, 2);
    }

    public static void ThreeByThreePositive(){
        double major = getMajorCoord(3);
        double minor = -major;
        ThreeCube(minor,major,major,major,major);
    }
}
