import { Bottle } from './bottle.model'

export interface Order {
    id: string;
    user: string;
    bottles: Array<Bottle>
    date: Date;
}