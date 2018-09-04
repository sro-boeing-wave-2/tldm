import {Message} from './Message';
import {User} from './User';

export class Channel{
  ChannelId:string;
  ChannelName:string;
  Users:User[];
  Admin:User;
  Messages:Message[];
  WorkspaceId:string;
 }
