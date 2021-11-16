# OslerAlumni

## Developer Orientation
* [Main Document](https://docs.google.com/document/d/10Zue2qgvTctVZ5gjPYrdCxkaSPh68nmCw8YmOGSjVV4/edit)

* [Useful Resources](https://basecamp.com/2151150/projects/15554761/documents/13568256)


## IIS

* Make sure you have URL Rewrite module installed
	* Select the name of you computer in the tree in IIS Manager
	* Check if "URL Rewrite" module is listed in the right-hand panel under "IIS" category
* If not, install the module before proceeding
	* [Installer for Windows 10](https://www.microsoft.com/en-us/download/details.aspx?id=47337)

### Admin site

* Create a site in IIS called `OslerAlumni-Admin`
 
    * Set physical path to `<Local Project Git Folder Root>\Alumni_Admin\CMS`

    * Set host name to `osleralumni-admin-lh.ecentricarts.com`

    * Go to `Application Pools` in IIS. Select `OslerAlumni-Admin`

    * Go to Advanced settings > `Identity`. Hit edit.

    * Select `Custom Account`. Hit Set.

    * Enter your Active Directory Credentials (your network login):

        * Enter Username : `<Domain>\Username`

        * Password: ***

* Ensure `IIS_IUSRS` has correct permissions

    * Go to Properties > Security on the `Alumni_Admin` folder

    * Set permissions for `IIS_IUSRS` to full control

* Add the following line to your `hosts` file:

    ```
    127.0.0.1    osleralumni-admin-lh.ecentricarts.com
    ```

* Navigate to `<Local Project Git Folder Root>\Alumni_Admin\CMS\`

* Configure your local web farm name:

    * Copy & paste the file `LocalSettings.config.template`

    * Rename the copy as `LocalSettings.config` and open it

    * Modify the `CMSWebFarmServerName` value to replace XX with your initials, e.g. for John Ramamoorthy you would modify "OslerAlumniAdminXX" to "OslerAlumniAdminJR" *(Note that the name of the web farm is different for Admin and MVC sites)*

* Open the VS solution at `<Local Project Git Folder Root>\Alumni_Admin\OslerAlumni_Admin.sln` (in Administrator mode) and rebuild it

    * Make sure that automatic restoring of Nuget packages is enabled in your VS instance

* Verify that the Admin site is setup correctly by browsing over to `osleralumni-admin-lh.ecentricarts.com`

* Kentico CMS Global Administrator: [see Developer Orientation](https://docs.google.com/document/d/10Zue2qgvTctVZ5gjPYrdCxkaSPh68nmCw8YmOGSjVV4/edit#heading=h.1peop8vv0tcj)

### MVC site

* Create a site in IIS called `OslerAlumni-Website`

    * Set physical path to `<Local Project Git Folder Root>\Alumni_Website\OslerAlumni.Mvc`

    * Set host name to `osleralumni-lh.ecentricarts.com`

    * Go to `Application Pools` in IIS. Select `OslerAlumni-Website`

    * Go to Advanced settings > `Identity`. Hit edit.

    * Select `Custom Account`. Hit Set.

    * Enter your Active Directory Credentials (your network login):

        * Enter Username : `<Domain>\Username`

        * Password: ***

* Ensure `IIS_IUSRS` has correct permissions

    * Go to Properties > Security on the `Alumni_Website` folder

    * Set permissions for `IIS_IUSRS` to full control

* Add the following line to your `hosts` file:

    ```
    127.0.0.1    osleralumni-lh.ecentricarts.com
	```

* Navigate to `<Local Project Git Folder Root>\Alumni_Website\OslerAlumni.Mvc\`

* Configure your local web farm name:

    * Copy & paste the file `LocalSettings.config.template`

    * Rename the copy as `LocalSettings.config` and open it

    * Modify the `CMSWebFarmServerName` value to replace XX with your initials, e.g. for John Ramamoorthy you would modify "OslerAlumniWebsiteXX" to "OslerAlumniWebsiteJR" *(Note that the name of the web farm is different for Admin and MVC sites)*

* Open the VS solution at `<Local Project Git Folder Root>\Alumni_Website\OslerAlumni_Website.sln` (in Administrator mode) and rebuild it

    * Make sure that automatic restoring of Nuget packages is enabled in your VS instance

* Verify that the MVC site is setup correctly by browsing over to `osleralumni-lh.ecentricarts.com`


## SSL

### Certificate generation

* On your local machine, find "Windows PowerShell" application and run it in administrator mode

* In the terminal, navigate to the `<Local Project Git Folder Root>\Scripts`, e.g.:

    ```
    cd C:\Projects\Osler\Alumni\Scripts
    ```

* Run the following script and make sure it completed without errors:

    ```
    .\CreateOslerAlumniCerts.ps1
    ```

* Open your local IIS and add the following bindings to your Admin and MVC sites:

    * Type: `https`

    * Host name:

        * (Admin site) `osleralumni-admin-lh.ecentricarts.com`

        * (MVC site) `osleralumni-lh.ecentricarts.com`

    * Require Server Name Indication: (check)

    * SSL certificate:

        * (Admin site) `osleralumni-admin-lh.ecentricarts.com`

        * (MVC site) `osleralumni-lh.ecentricarts.com`


## Front-end build tools

### Init

`npm install`

For full list of packages, see `package.json` in root.


### Webpack

This project is compiled using webpack. Configuration is in `webpack.config.js` in the root.

Project config settings (like paths) are in `project.config`


### Available command line options:

`webpack` or `npm run dev` -  default command. Cleans build folder and runs babel, js bundler and sass compiler.

`npm run watch` - runs the above and watches for changes.

`npm run build` - for production. Cleans build folder and runs babel, js bundler, js uglify, sass compiler, and sass minification.

`npm run clean` - cleans build folder.

(see `scripts` in `package.json` for details)


### Requirements

node >= `6.11.5`

npm `4.20.2`


### Notes

* CSS is loosley using a version of BEMIT/ITCSS methodology
* `node-sass` (in the form of loader `sass-loader`) is run on dev as it is fairly quick.
* Foundation loaded via npm foundation-sites 6.5.0-rc.3
* The CSS for CKEditor is generate by webpack into adminStyles.css in the build folder. These styles need to be manually added to the CMS under `Sites > General`. Copy and paste the styles from the built file in to the Editor CSS stylesheet box.


## Project Architecture

* Back-end project structure:
    * [Default project folder structure](https://docs.google.com/spreadsheets/d/1PPeFuVoYOxZtQ70aU3Voqj5K5Z-w5Q-N4Kd0iByuft0/edit#rangeid=863977693)

    * [Admin site solution](https://docs.google.com/spreadsheets/d/1PPeFuVoYOxZtQ70aU3Voqj5K5Z-w5Q-N4Kd0iByuft0/edit#gid=0)

    * [MVC site solution](https://docs.google.com/spreadsheets/d/1PPeFuVoYOxZtQ70aU3Voqj5K5Z-w5Q-N4Kd0iByuft0/edit#gid=911297052)

    * [Shared libraries](https://docs.google.com/spreadsheets/d/1PPeFuVoYOxZtQ70aU3Voqj5K5Z-w5Q-N4Kd0iByuft0/edit#gid=334203220)

    * [Tests](https://docs.google.com/spreadsheets/d/1PPeFuVoYOxZtQ70aU3Voqj5K5Z-w5Q-N4Kd0iByuft0/edit#gid=1225349416)

* Front-end folder structure:

    ```
    [root]
    |-- Alumni_Website
    |   |-- OslerAlumni.Mvc
    |   |   |-- build
    |   |   |   |-- img // optimized image assets
    |   |   |   |   -- [area of usage] // e.g. "brand", "flag", "icon", etc.
    |   |   |   |   |   `-- img1.[png/svg/etc.]
    |   |   |   |-- scripts.js //compiled js
    |   |   |   `-- styles.css //compiled css
    |   |   |-- src
    |   |   |   |-- img // image assets
    |   |   |   |   |-- [area of usage] // e.g. "brand", "flag", "icon", etc.
    |   |   |   |   |   |-- img1.[png/svg/etc.]
    |   |   |   |-- js
    |   |   |   |   |-- components
    |   |   |   |   |   |-- component1.js
    |   |   |   |   |   |-- component2.js
    |   |   |   |   |   `-- etc.
    |   |   |   |   |-- vendor // third-party libraries
    |   |   |   |   `--scripts.js // js entry point
    |   |   |   |-- scss
    |   |   |   |   |-- partials
    |   |   |   |   |   |-- _core.scss // import foundation and foundation settings
    |   |   |   |   |   |-- _settings.scss // customize foundation settings here
    |   |   |   |   |   `-- etc. // all other partials
    |   |   |   |   `-- styles.scss // css/sass entry point
    ```

## API
* MVC site has Swagger UI enabled for it that provides an easy way of viewing and testing API endpoints: https://osleralumni-lh.ecentricarts.com/api/swagger
	* Note 1: Swagger UI only works with Web API controller actions, not MVC
	* Note 2: You need to be logged in on the MVC site, in order to access API endpoints that require authentication

## Unit Tests:
XUnit has been added as a testing framework.

Unit Testing has been enabled by default on Visual Studio

If you have Visual Studio Community (or a paid-for version of Visual Studio), you can run your xUnit.net tests within Visual Studio's built-in test runner (named Test Explorer). Unfortunately, this does not include Express editions of Visual Studio (you should upgrade to the free Community Edition instead).

### How to run unit tests:
* Make sure `Test Explorer` is visible (go to Test > Windows > Test Explorer). Every time you build your project, the runner will discover unit tests in your project. After a moment of discovery, you should see the list of discovered tests.
* Click the Run All link in the Test Explorer window, and you should see the results update in the Test Explorer window as the tests are run:
* ![alt text](https://xunit.github.io/images/running-tests-in-vs/test-explorer-post-run.png)

## Third Party Libraries Used:

#### IPAddressRange Class Library:

https://github.com/jsakamoto/ipaddressrange

Used for detectings users present on the Osler Network.

#### Jcrop:

https://jcrop.com/

Javascript library for Image Cropping.

## TODOs

