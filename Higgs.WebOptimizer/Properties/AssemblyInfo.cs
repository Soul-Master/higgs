﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Higgs.WebOptimizer")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Higgs.WebOptimizer")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("e62058af-b776-415b-b5c2-1569f7770640")]


#region Assembly Version
/*
 * File Version Format(AssemblyVersion)
 * 
 * This is the actual file version (set by using the AssemblyFileVersionAttribute attribute) and 
 * it satisfies the following goals:
 * - It correlates the product binaries to the source files from which they were compiled 
 *   (as long as labeling is performed in the source code control database)
 * - It allows for the re-creation of older builds.
 * - It clearly identifies upgrade and bug fix releases.
 * - It clearly identifies which version of the source code is in production.
 *  
 * <major>.<minor>.<buildnumber>.<revision>
 * 
 * The File Version could follow this format:
 * - Major          This is the internal version of the product and is assigned by the application team.
 *                  It should not change during the development cycle of a product release.
 * - Minor          This is normally used when an incremental release of the product is planned 
 *                  rather than a full feature upgrade.  It is assigned by the application team, 
 *                  and it should not be changed during the development cycle of a product release.
 * - Build Number   The build process usually generates this number.  Keep in mind that the numbers 
 *                  cannot be more than 2^16.
 * - Revision       This could be assigned by the build team and could contain the reference number of 
 *                  the migration to the production environment. When it is not known yet, a 0 is used
 *                  until the number is issued.
 *                  
 *          Build Number = amount of normal building solution
*/
#endregion
[assembly: AssemblyVersion("1.1.1.0")]

#region Assembly Informational Version
/*
 * Product Number Format (AssemblyInformationalVersion) 
 *      This is the product number that is communicated to stakeholders outside
 * the development and build teams
 * 
 *   <major>.<minor>[.<revision>] <stage> <build number>.
 * 
 * - Major          Increment this number when major functionality is being released
 * - Minor          Increment this number when alterations or enhancements to existing
 *                  functionality is made and changes the end user experience.
 * - Stage
 *  1. Pre-Alpha    The pre-alpha is not feature complete.
 *  2. Alpha        The alpha build of the software is the build to the internal software testers.
 *  3. Beta         Software which has passed the alpha testing stage of development and has been 
 *                  released to users for software testing before its official release.
 *  4. RC           Release candidate (RC) refers to a version with potential to be a final product,
 *                  ready to release unless fatal bugs emerge
 *  5. RTM          The term "release to manufacturing" or "release to marketing"(RTM) is used to 
 *                  indicate that the software has met a defined quality level and is ready for 
 *                  mass distribution either by electronic means or by physical media.          
 * 
 * - Build Number   The build process usually generates this number. A difference in build number 
 *                  represents a recompilation of the same source. This would be appropriate because 
 *                  of processor, platform, or compiler changes.
 * 
*/
#endregion
[assembly: AssemblyInformationalVersion("1.1")]