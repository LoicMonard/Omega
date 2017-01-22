import { getAsync, postAsync } from '../helpers/apiHelper'
import AuthService from './AuthService'

const endpoint = "/api/EventGroup";

class FacebookApiService {
    constructor() {

    }

    async getFacebookEvents() {
        return await getAsync(endpoint, 'RetrieveUserEvents', AuthService.accessToken);
    }
    async getFacebookGroups() {
        return await getAsync(endpoint, 'RetrieveUserGroups', AuthService.accessToken);
    }
    async createEvent(event) {
        return await postAsync(endpoint, 'CreateEvent', AuthService.accessToken, event);
    }
}

export default new FacebookApiService()