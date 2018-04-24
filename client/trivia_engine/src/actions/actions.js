import * as types from './actionsTypes';

function url() {
  return 'http://localhost:1234';
}

export function receiveAnswer(json) {
  return {type: types.ANSWER_RECEIVED, payload: json};
}

export function fetchStuff(data) {
    console.log(JSON.stringify(data));
  return dispatch => {
    return fetch('/answer', {
      method: 'POST',
      headers: new Headers({
        'Content-Type': 'application/json',
        Accept: 'application/json',
      }),
      body: JSON.stringify(data)
    })
    .then(response => {
        console.log(response)
        return response.json()
    })
    .then(json => dispatch(receiveAnswer(json)));
  };
}