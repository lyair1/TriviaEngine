import * as types from './actionsTypes';

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
        let json = response ? response.json() : {};
        return json;
    })
    .then(json => {
        console.log(json)
        dispatch(receiveAnswer(json))
      }).catch((error) => {
        console.log(error);
      });
  };
}