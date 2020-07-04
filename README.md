# DynaConv
An automation script for LavenderVR that converts `DynamicBone` and `DynamicBoneCollider` into `JiggleBone` and `JiggleColliderSphere`.

![Preview](https://i.imgur.com/4xbnKwf.png)

# Features
 * Can remove or disable Dynamic Bones' components when done porting over
 * Can update Lavender's proxy component values from Dynamic Bones' components (if you kept them)

# Installation
Simply [grab the latest release](https://github.com/Rikketh/Unity-Lavender-DynaConv/releases/latest) or download the repo, then throw `DynaConv` folder somewhere into your project's `Assets` folder. If everything's fine, you should see a new component when searching for "Dyna". Add the component, configure it, then hit the big button to convert everything.