'use strict';

const endpoint = "https://www.google.com/search?&q=";
const axios = require('axios')
const cheerio = require('cheerio')

var getResultsCountFromHtml = function(res) {
  let $ = cheerio.load(res.data)

  return parseInt($('#resultStats')[0].children[0].data.split(' ')[1].replace(/,/g, ""));
}

var howManyResults = function(query, aIdx) {
  return new Promise(function(resolve, reject) {
    axios.get(endpoint + query.replace(/ /g, "+"))
    .then(function (response) {
      let result = {
        aIdx,
        totalResults: getResultsCountFromHtml(response)
      }
      resolve(result)
    })
    .catch(function (error) {
      console.log(error)
      let result = {
        aIdx,
        totalResults: -1
      }
      resolve(result)
    });
  });
}

function occurrences(string, subString, allowOverlapping) {

  string += "";
  subString += "";
  if (subString.length <= 0) return (string.length + 1);

  var n = 0,
      pos = 0,
      step = allowOverlapping ? 1 : subString.length;

  while (true) {
      pos = string.indexOf(subString, pos);
      if (pos >= 0) {
          ++n;
          pos += step;
      } else break;
  }
  return n;
}

var countAnswerInResponse = function(query, answers) {
  return new Promise(function(resolve, reject) {
    axios.get(endpoint + query)
    .then(function (response) {
      let lowerData = JSON.stringify(response.data).toLowerCase()
      let countArr = [0, 0, 0]

      countArr[0] = occurrences(lowerData, answers[0].trim().toLowerCase(), false);
      countArr[1] = occurrences(lowerData, answers[1].trim().toLowerCase(), false);
      countArr[2] = occurrences(lowerData, answers[2].trim().toLowerCase(), false);

      console.log(JSON.stringify(countArr));

      resolve(countArr)
    })
    .catch(function (error) {
      console.log(error)
      resolve([0, 0, 0])
    });
  });
}

var getProbabilities = function(scoresArray) {
  let totalCount = scoresArray.reduce((accumulator, currentValue) => accumulator + currentValue);
  
  if (totalCount === 0) {
    return {
      '0' : 0,
      '1' : 0,
      '2' : 0,
    }
  }

  return {
    '0' : (scoresArray[0] / totalCount).toFixed(3),
    '1' : (scoresArray[1] / totalCount).toFixed(3),
    '2' : (scoresArray[2] / totalCount).toFixed(3),
  }
}

var printStatistics = function(scoresArray) {
  let prob = getProbabilities(scoresArray);

  console.log('#1: ' + prob['0'] + ', #2: ' + prob['1'] + ', #3: ' + prob['2'])
}

exports.findAnswer = function(req, res) {
    console.log('req:' + JSON.stringify(req.body))
    var promises = [];

    let question = req.body.question;
    let answer1 = req.body.answer1;
    let answer2 = req.body.answer2;
    let answer3 = req.body.answer3;

    // scores array
    let scoresArray = [0, 0, 0]

    let waitCount = 0;

    // Case #1: question + answer -> compare total results
    var case1Factor = 5;

    var query1 = question + ' ' + answer1  
    var query2 = question + ' ' + answer2
    var query3 = question + ' ' + answer3

    promises.push(howManyResults(query1, 0));
    promises.push(howManyResults(query2, 1));
    promises.push(howManyResults(query3, 2));

    let totalResults = [0, 0, 0]

    waitCount++;
    Promise.all(promises)
    .then(function (responses) {
      responses.map(x => totalResults[x.aIdx] = parseInt(x.totalResults));

      let totalCount = totalResults.reduce((accumulator, currentValue) => accumulator + currentValue)
    
      scoresArray[0] += (totalResults[0] / totalCount) * case1Factor;
      scoresArray[1] += (totalResults[1] / totalCount) * case1Factor;
      scoresArray[2] += (totalResults[2] / totalCount) * case1Factor;

      printStatistics(scoresArray);

      waitCount--;
      if (waitCount <= 0) {
        let data = {
          'prob' : getProbabilities(scoresArray),
          'question' : question,
          'answer1' : answer1,
          'answer2' : answer2,
          'answer3' : answer3
        };
        res.json(data)
      }
    })
    .catch(function (error) {
      console.log(error);
      res.json({'status' : 'failed'});
    });

    // Case #2: question -> count answer appearance in results
    var case2Factor = 10;
    waitCount++;
    countAnswerInResponse(question, [answer1, answer2, answer3])
    .then(function (response) {
      let totalCount = response.reduce((accumulator, currentValue) => accumulator + currentValue)
      if (totalCount > 0) {
        scoresArray[0] += (response[0] / totalCount) * case2Factor;
        scoresArray[1] += (response[1] / totalCount) * case2Factor;
        scoresArray[2] += (response[2] / totalCount) * case2Factor;
      }
        
      printStatistics(scoresArray);

      waitCount--;
      if (waitCount <= 0) {
        let data = {
          'prob' : getProbabilities(scoresArray),
          'question' : question,
          'answer1' : answer1,
          'answer2' : answer2,
          'answer3' : answer3
        };
        res.json()
      }
    })
    .catch(function (error) {
      console.log(error);
      res.json({'status' : 'failed'});
    });
  };


