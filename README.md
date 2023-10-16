# unity-jenga-gtschool
Crossover Submission for GtSchool by Abdurrafay Bin Khurram

![Unity Version](https://img.shields.io/badge/Unity-2021.3.9f1-blue?&logo=unity)
![GitHub last commit](https://img.shields.io/github/last-commit/rafay-pk/unity-jenga-gtschool?)
![GitHub issues](https://img.shields.io/github/issues/rafay-pk/unity-jenga-gtschool?)
![GitHub closed issues](https://img.shields.io/github/issues-closed/rafay-pk/unity-jenga-gtschool?)
![GitHub repo size](https://img.shields.io/github/repo-size/rafay-pk/unity-jenga-gtschool?)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/rafay-pk/unity-jenga-gtschool?)

You can find the latest build in `/docs` folder or view it online [here](https://rafay-pk.github.io/unity-jenga-gtschool/).

<span style="color: red;">Note: </span>The API request to get the data is working in the Unity editor but fails on WEBGL builds due to CORS policy. This is a setting that has to be changed from the server side of the API. That's why I've uploaded a build that utilizes the data from a local JSON file.

To run the build locally, you can run a local server using python. Just run the following command in the `/docs` folder of the cloned repo:

``` bash
python -m http.server 8000
```
Then open [localhost:8000](localhost:8000) in your browser.