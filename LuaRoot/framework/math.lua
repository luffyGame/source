--一些数学属性，比如绝对值,pi啥的
local Format = string.format
local random,abs,sqrt = math.random,math.abs,math.sqrt
local acos	= math.acos
local cos,sin = math.cos,math.sin
local _G = _G

local rad2Deg = 57.295779513082
local deg2Rad = 0.017453292519943

local Math = {}

function Math.Clamp(val,min,max)
	if min~=nil and val < min then
		return min
	end
	if max~=nil and val > max then
		return max
	end
	return val
end
--accuracy，精度
function Math.EqualZero(val,accuracy)
	if abs(val) < (accuracy or 0.0001) then
		return true
	end
	return false
end

function Math.Chance10K(val)
	return random(1,10000)<=val
end

function Math.RandomF(min,max)
	return min + random()*(max-min)
end

_G.Math = Math

---------------------------三维向量，用于坐标和方向-----------------------------------
local Vector3 = class("Vector3")

function Vector3:ctor(_x,_y,_z)
	self.x = _x
	self.y = _y
	self.z = _z
end

function Vector3:toString()
	return Format("{x=%0.3f,y=%0.3f,z=%0.3f}",self.x,self.y,self.z)
end

function Vector3.zero()
	return Vector3.new(0,0,0)
end

function Vector3.one()
	return Vector3.new(1,1,1)
end

function Vector3.forward()
	return Vector3.new(0,0,1)
end

function Vector3.right()
	return Vector3.new(1,0,0)
end

function Vector3.up()
	return Vector3.new(0,1,0)
end

function Vector3:set(_x,_y,_z)
	self.x = _x
	self.y = _y
	self.z = _z
	return self
end

function Vector3:add(pos3d)
	self.x = self.x + pos3d.x
	self.y = self.y + pos3d.y
	self.z = self.z + pos3d.z
	return self
end

function Vector3:sub(pos3d)
	self.x = self.x - pos3d.x
	self.y = self.y - pos3d.y
	self.z = self.z - pos3d.z
	return self
end

function Vector3:mul(num)
	self.x = self.x * num
	self.y = self.y * num
	self.z = self.z * num
	return self
end

function Vector3:copy(src)
	self:set(src.x,src.y,src.z)
	return self
end

function Vector3:equal(src,accuracy)
	if not src then
		return false
	end
	return Math.EqualZero(self.x-src.x,accuracy) and Math.EqualZero(self.y-src.y,accuracy) and
		Math.EqualZero(self.z - src.z,accuracy)
end

function Vector3:equalXyz(x,y,z,accuracy)
	return Math.EqualZero(self.x-x,accuracy) and Math.EqualZero(self.y-y,accuracy) and
		Math.EqualZero(self.z-z,accuracy)
end

function Vector3:isZero(accuracy)
	return self:equalXyz(0,0,0,accuracy)
end

function Vector3:equalXz(src,accuracy)
	return Math.EqualZero(self.x-src.x,accuracy) and	Math.EqualZero(self.z-src.z,accuracy)
end

function Vector3:setNormalize()
	local num = self:magnitude()
	if num > 0.00001 then
		self.x = self.x/num
		self.y = self.y/num
		self.z = self.z/num
	else
		self.x = 0
		self.y = 0
		self.z = 0
	end
	return self
end

function Vector3:normalize()
	local num = self:magnitude()
	if num > 0.00001 then
		return Vector3.new(self.x/num,self.y/num,self.z/num)
	else
		return Vector3.zero()
	end
end


function Vector3:magnitude()
	return sqrt(self.x*self.x+self.y*self.y+self.z*self.z)
end

function Vector3:mag2()
	return self.x*self.x+self.y*self.y+self.z*self.z
end

function Vector3:magnitudeH() --忽略高度
	return sqrt(self.x*self.x+self.z*self.z)
end

function Vector3:clone()
	return Vector3.new(self.x,self.y,self.z)
end

Vector3.__add = function(vec1,vec2)
	return Vector3.new(vec1.x+vec2.x,vec1.y+vec2.y,vec1.z+vec2.z)
end

Vector3.__sub = function(vec1,vec2)
	return Vector3.new(vec1.x-vec2.x,vec1.y-vec2.y,vec1.z-vec2.z)
end

Vector3.__mul = function (vec1,num)
	return Vector3.new(vec1.x*num,vec1.y*num,vec1.z*num)
end

Vector3.__unm = function(vec)
	return Vector3.new(-vec.x,-vec.y,-vec.z)
end

function Vector3:inverse()
	self.x = -self.x
	self.y = -self.y
	self.z = -self.z
	return self
end

function Vector3:dist(rhs)
	local xdelta = self.x - rhs.x
	local ydelta = self.y - rhs.y
	local zdelta = self.z - rhs.z
	return sqrt(xdelta*xdelta+ydelta*ydelta+zdelta*zdelta)
end

function Vector3:distNH(rhs)
	local xdelta = self.x - rhs.x
	local zdelta = self.z - rhs.z
	return sqrt(xdelta*xdelta+zdelta*zdelta)
end

--绕y轴的旋转弧度
function Vector3:yRad()
	local mh = self:magnitudeH()
	if mh < 0.00001 then return 0 end
	return acos(Math.Clamp(self.x/mh,-1,1))
end

function Vector3:yRotate(yCos,ySin)
	local ox,oz = self.x,self.z
	self.x = ox*yCos - oz*ySin
	self.z = ox*ySin + oz*yCos
end

function Vector3:yRotateAngle(yRad)
	self:yRotate(cos(yRad),sin(yRad))
end

function Vector3.Dot(lhs, rhs)
	return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z
end

function Vector3.Angle(from, to)
	return acos(Math.Clamp(Vector3.Dot(from:normalize(), to:normalize()), -1, 1)) * rad2Deg
end

function Vector3.Angle1(fromNorm,to)
	return acos(Math.Clamp(Vector3.Dot(fromNorm, to:normalize()), -1, 1)) * rad2Deg
end

_G.Vector3 = Vector3

------------------------------二维向量-----------------------------
local Vector2 = class("Vector2")

function Vector2:ctor(_x,_y)
	self.x = _x
	self.y = _y
end

function Vector2:toString()
	return Format("{x=%0.3f,y=%0.3f}",self.x,self.y)
end

function Vector2:set(_x,_y)
	self.x = _x
	self.y = _y
	return self
end

function Vector2:copy(src)
	return self:set(src.x,src.y)
end

function Vector2:add(pos2d)
	self.x = self.x + pos2d.x
	self.y = self.y + pos2d.y
	return self
end

function Vector2:sub(pos2d)
	self.x = self.x - pos2d.x
	self.y = self.y - pos2d.y
	return self
end

function Vector2:magnitude()
	return sqrt(self.x*self.x+self.y*self.y)
end

function Vector2:sqrMagnitude()
	return self.x*self.x+self.y*self.y
end

function Vector2:clone()
	return Vector2.new(self.x,self.y)
end

function Vector2.zero()
	return Vector2.new(0,0)
end

_G.Vector2 = Vector2