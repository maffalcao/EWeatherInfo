# EWeatherInfo
A REST API that get information from a Blob Storage and deliver it in a structured way

### Installation instructions

Having docker installed on the computer, access the src folder of the project and run the command:

`docker compose up`


### Instructions for accessing endpoints

Get measurements informing the *deviceId*, *sensorType* and *date*:

    curl --location --request GET 'http://localhost:8001/api/v1/WeatherInfo/?deviceId={{deviceId}}&sensorType={sensorType}&date={{date}}'

Get measurements informing the *deviceId* and *date*:

    curl --location --request GET 'http://localhost:8001/api/v1/WeatherInfo/?deviceId={{deviceId}}&date={{date}}'


Being:

- **DeviceId**: required. Is the name of the deviceId
- **Date**: mandatory. It is the date on which you want to search for measurements
- **SensorType**: required. It is the name of the sensorType that you want to search for measurements for a given deviceId


Expected return:

    [
    	{
    		name: {{sensorType}}
    		measurements: [
    			{ Date: "2019-01-01 13:52", Value: 0.2},
    			{ Date: "2019-01-01 13:57", Value: 0.3},
    			...
    		]
    	}	
    	// if the sensorType is not specified (2) the other sensors displayed in the result	
    	{
    		name: {{sensorType2}}
    		measurements: [
    			{ Date: "2019-01-01 13:52", Value: 0.2},
    			{ Date: "2019-01-01 13:57", Value: 0.3},
    			...
    		]
    	}
    	...
    ]
	

------------



### Application flow

The application flow is as shown in the diagram below:

![alt text](https://i.postimg.cc/xTwcrT6x/Measurements-Final.png)

Given a *deviceId*, *sensorType* and a *date*

1. It is verified if the deviceId is registered and if it has the informed sensorType (metadata.css)

2. Checked in cash if there is data for this set of keys
3. If there is no cached data, it is checked if the csv is available in the local directory (temporary folder). If yes, data is retrieved, processed and delivered
4. If the csv is not in the local directory, it is checked if the csv url exists. If yes, it is downloaded to the local directory and, after that, the data is retrieved, processed and delivered.
5. If the csv url does not exist, it is checked if there is a .zip url referring to that history. If yes, it is downloaded, unzipped in the local directory. Afterwards, the csv existence is verified in the local directory for that date. If yes, the data is retrieved, processed and delivered.

**Improvement points for the next version:**

1. When the stream enters the .zip download and unzip step, the request tends to take several minutes to complete. I saw some strategies to make the download faster by breaking the download stream into chunks and downloading them in parallel.

1. Also, when the sensorType is not specified in the request, it is necessary to iterate over all sensorTypes of that device to retrieve its measurements. For each sensorType, the flow mentioned above runs. One way to speed up the process would be to query for each sensorType in parallel.